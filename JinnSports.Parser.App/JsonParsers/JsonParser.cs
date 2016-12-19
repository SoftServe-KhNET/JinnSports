using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using log4net;
using Newtonsoft.Json;
using JinnSports.DAL.Repositories;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using JinnSports.Parser.App.Exceptions;
using JinnSports.Parser.App.Interfaces;
using JinnSports.Parser.App.JsonParsers.JsonEntities;
using JinnSports.Parser.App.ProxyService.ProxyConnection;

namespace JinnSports.Parser.App.JsonParsers
{
    public enum Locale
    {
        En, RU
    }

    public class JsonParser : ISaver
    {
        private static readonly ILog Log = 
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IUnitOfWork uow;

        public JsonParser() : this(new Uri("http://results.fbwebdn.com/results.json.php"))
        {
        }

        public JsonParser(Uri uri) : this(uri, new EFUnitOfWork("SportsContext"))
        {
        }

        public JsonParser(IUnitOfWork unit) :
            this(new Uri("http://results.fbwebdn.com/results.json.php"), new EFUnitOfWork("SportsContext"))
        {
        }

        public JsonParser(Uri uri, IUnitOfWork unit)
        {
            this.SiteUri = uri;
            this.uow = unit;
        }

        public Uri SiteUri { get; private set; }

        public void StartParser()
        {
            Log.Info("Json parser was started");
            try
            {
                string result;
                JsonResult jsonResults;
                List<Result> res;

                try
                {
                    result = this.GetJsonFromUrl(this.SiteUri);
                }
                catch (Exception ex)
                {
                    throw new WebResponseException(ex.Message, ex.InnerException);
                }

                try
                {
                    jsonResults = this.DeserializeJson(result);
                }
                catch (Exception ex)
                {
                    throw new JsonDeserializeException(ex.Message, ex.InnerException);
                }

                try
                {
                    res = this.GetResultsList(jsonResults);
                }
                catch (Exception ex)
                {
                    throw new ParseException(ex.Message, ex.InnerException);
                }

                try
                {
                    this.DBSaveChanges(res);
                    Log.Info("New data from json parser was saved to DataBase");
                }
                catch (Exception ex)
                {
                    throw new SaveDataException(ex.Message, ex.InnerException);
                }
            }
            catch (WebResponseException ex)
            {
                Log.Error(ex);
            }
            catch (JsonDeserializeException ex)
            {
                Log.Error(ex);
            }
            catch (ParseException ex)
            {
                Log.Error(ex);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public string GetJsonFromUrl()
        {
            return this.GetJsonFromUrl(this.SiteUri);
        }

        public string GetJsonFromUrl(Uri uri, Locale locale = Locale.RU)
        {
            int ch;
            string result = string.Empty;
            ProxyConnection pc = new ProxyConnection();
            string url = string.Format("{0}?locale={1}", uri.ToString(), locale == Locale.En ? "en" : "ru");
            HttpWebResponse resp = pc.MakeProxyRequest(uri.ToString(), 0);
            if (resp == null)
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                resp = (HttpWebResponse)req.GetResponse();
            }
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);

            while (!sr.EndOfStream)
            {
                result += sr.ReadLine();
            }

            resp.Close();
            return result;
        }

        public JsonResult DeserializeJson(string jsonStr)
        {
            JsonResult res = JsonConvert.DeserializeObject<JsonResult>(jsonStr);
            return res;
        }

        public void DBSaveChanges(List<Result> results)
        {
            this.uow.GetRepository<Result>().InsertAll(results.ToArray());
            this.uow.SaveChanges();
        }

        public List<Result> GetResultsList(JsonResult result)
        {
            List<Result> resultList = new List<Result>();
            List<SportType> sportList = new List<SportType>();
            foreach (var e in result.Events)
            {
                Team team1 = new Team();
                Team team2 = new Team();
                if (this.GetTeamsFromEvent(e, team1, team2))
                {
                    SportType sportType = new SportType();
                    Result resTeam1 = new Result();
                    Result resTeam2 = new Result();
                    SportEvent compEvent = new SportEvent { Date = this.GetEventDate(e) };

                    var sports = result.Sections.Where(n => n.Events.Contains(e.Id)).Select(n => n).ToList();
                    string sportName = result.Sports.Where(n => n.Id == sports[0].Sport).Select(n => n).ToList()[0].Name;
                    if (this.CheckSportTypeExist(sportName))
                    {
                        sportType = this.uow.GetRepository<SportType>().Get().Where(n => n.Name == sportName).ToList()[0];
                        sportList.Add(sportType);
                    }
                    else
                    {
                        if (sportList.Where(n => n.Name == sportName).Count() > 0)
                        {
                            sportType = sportList.Where(n => n.Name == sportName).ToList()[0];
                        }
                        else
                        {
                            sportType.Name = result.Sports.Where(n => n.Id == sports[0].Sport).Select(n => n).ToList()[0].Name;
                            sportList.Add(sportType);
                        }
                    }

                    team1.SportType = sportType;
                    team2.SportType = sportType;
                    compEvent.SportType = sportType;

                    this.GetResultFromEvent(e, resTeam1, team1, false);
                    this.GetResultFromEvent(e, resTeam2, team2, true);
                    resTeam1.SportEvent = compEvent;
                    resTeam2.SportEvent = compEvent;

                    resultList.Add(resTeam1);
                    resultList.Add(resTeam2);
                }
            }
            return resultList;
        }

        private void GetResultFromEvent(Event ev, Result res, Team team, bool invertScore)
        {
            res.Team = team;
            string mainScore;
            int score;

            if (ev.Score.Contains("("))
            {
                mainScore = ev.Score.Substring(0, ev.Score.IndexOf('('));
            }
            else
            {
                mainScore = ev.Score;
            }

            string[] scores = mainScore.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            if (!invertScore)
            {
                int.TryParse(scores[0], out score);
            }
            else
            {
                int.TryParse(scores[1], out score);
            }
            res.Score = score;
        }

        private bool GetTeamsFromEvent(Event ev, Team team1, Team team2)
        {
            if (ev.Name.Contains("-") && !ev.Name.Contains(":") && !ev.Name.Contains("1st")
                && !ev.Name.Contains("2nd") && ev.Status == 3 && ev.Score.Contains(":"))
            {
                string[] teams = ev.Name.Split(new char[] { '-' }, StringSplitOptions.None);
                team1.Name = teams[0];
                team2.Name = teams[1];
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckSportTypeExist(string name)
        {
            List<SportType> stypeList = this.uow.GetRepository<SportType>().Get().ToList();
            foreach (var type in stypeList)
            {
                if (type.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        private DateTime GetEventDate(Event ev)
        {
            int startTime = (int)ev.StartTime;
            int hour, min;
            hour = (startTime / 60 / 60) % 24;
            min = (startTime / 60) % 60;
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, min, 0);
        }
    }
}
