using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using JinnSports.DAL.Repositories;
using JinnSports.Entities.Entities;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Parser.App.Interfaces;
using JinnSports.Parser.App.JsonParsers.JsonEntities;
using JinnSports.Parser.App.ProxyService.ProxyConnection;

namespace JinnSports.Parser.App.JsonParsers
{
    public class JsonParser : ISaver
    {
        private IUnitOfWork uow;

        public Uri SiteUri { get; private set; }

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

        public void StartParser()
        {
            JsonResult jResults = this.DeserializeJson(this.GetJsonFromUrl(this.SiteUri));
            List<Result> res = this.GetResultsList(jResults);
            this.DBSaveChanges(res);
        }

        public string GetJsonFromUrl()
        {
            return this.GetJsonFromUrl(this.SiteUri);
        }

        public string GetJsonFromUrl(Uri uri)
        {
            int ch;
            string result = string.Empty;
            ProxyConnection pc = new ProxyConnection();
            HttpWebResponse resp = pc.MakeProxyRequest(uri.ToString(), 4);
            if (resp == null)
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(this.SiteUri);
                resp = (HttpWebResponse)req.GetResponse();
                Console.WriteLine("Proxy wasnt used");
            }
            Stream stream = resp.GetResponseStream();

            for (int i = 1; ; i++)
            {
                ch = stream.ReadByte();
                if (ch == -1)
                {
                    break;
                }
                result += (char)ch;
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
            uow.Set<Result>().AddAll(results.ToArray());
            uow.SaveChanges();
        }

        public List<Result> GetResultsList(JsonResult result)
        {
            List<Result> resultList = new List<Result>();
            List<SportType> sportList = new List<SportType>();
            foreach (var e in result.Events)
            {
                Team team1 = new Team() { Results = new List<Result>() };
                Team team2 = new Team() { Results = new List<Result>() };
                if (this.GetTeamsFromEvent(e, team1, team2))
                {
                    SportType sportType = new SportType() { Teams = new List<Team>() };
                    Result resTeam1 = new Result();
                    Result resTeam2 = new Result();
                    CompetitionEvent compEvent = new CompetitionEvent() { Date = this.GetEventDate(e), Results = new List<Result>() };

                    var sports = result.Sections.Where(n => n.Events.Contains(e.Id)).Select(n => n).ToList();
                    string sportName = result.Sports.Where(n => n.Id == sports[0].Sport).Select(n => n).ToList()[0].Name;
                    if (sportList.Where(n => n.Name == sportName).Count() > 0)
                    {
                        sportType = sportList.Where(n => n.Name == sportName).ToList()[0];
                    }
                    else
                    {
                        sportType.Name = result.Sports.Where(n => n.Id == sports[0].Sport).Select(n => n).ToList()[0].Name;
                        sportList.Add(sportType);
                    }
                    sportType.Teams.Add(team1);
                    sportType.Teams.Add(team2);

                    team1.SportType = sportType;
                    team2.SportType = sportType;

                    this.GetResultFromEvent(e, resTeam1, team1, false);
                    this.GetResultFromEvent(e, resTeam2, team2, true);
                    resTeam1.CompetitionEvent = compEvent;
                    resTeam2.CompetitionEvent = compEvent;

                    team1.Results.Add(resTeam1);
                    team2.Results.Add(resTeam2);

                    compEvent.Results.Add(resTeam1);
                    compEvent.Results.Add(resTeam2);

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
                res.Score = scores[0];
            }
            else
            {
                res.Score = scores[1];
            }
        }

        private bool GetTeamsFromEvent(Event ev, Team team1, Team team2)
        {
            if (ev.Name.Contains("-") && !ev.Name.Contains(":") && !ev.Name.Contains("1st")
                && !ev.Name.Contains("2nd") && ev.Status == 3)
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
