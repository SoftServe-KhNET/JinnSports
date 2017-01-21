using DTO.JSON;
using log4net;
using Newtonsoft.Json;
using JinnSports.Parser.App.Exceptions;
using JinnSports.Parser.App.JsonParsers.JsonEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using JinnSports.Parser.App.ProxyService.ProxyConnections;
using JinnSports.Parser.App.ProxyService.ProxyTerminal;
using JinnSports.Parser.App.ProxyService.ProxyInterfaces;
using JinnSports.Parser.App;

namespace JinnSports.Parser.App.JsonParsers
{
    public enum Locale
    {
        EN, RU
    }

    public class JsonParser
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IProxyTerminal proxyTerminal;

        public JsonParser() : this(new Uri("http://results.fbwebdn.com/results.json.php"))
        {
            proxyTerminal = new ProxyTerminal();
        }

        public JsonParser(Uri uri)
        {
            proxyTerminal = new ProxyTerminal();
            this.SiteUri = uri;
        }

        public Uri SiteUri { get; private set; }

        public void StartParser()
        {
            Log.Info("Json parser was started");
            try
            {
                string result = this.GetJsonFromUrl(this.SiteUri);
                JsonResult jsonResults = this.DeserializeJson(result);
                List<SportEventDTO> sportEventsList = this.GetSportEventsList(jsonResults);
                this.SendEvents(sportEventsList);

                Log.Info("New data from JSON parser was sent");
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
            string result = string.Empty;
            HttpWebResponse response;
            Stream stream = null;

            string url = string.Format("{0}?locale={1}", uri.ToString(), locale == Locale.EN ? "en" : "ru");

            try
            {
                response = this.proxyTerminal.GetProxyResponse(uri);
                stream = response.GetResponseStream();
            }
            catch (Exception ex)
            {
                throw new WebResponseException(ex.Message, ex.InnerException);
            }

            using (StreamReader sr = new StreamReader(stream))
            {
                while (!sr.EndOfStream)
                {
                    result += sr.ReadLine();
                }
            }
            //resp.Close();

            return result;
        }

        public JsonResult DeserializeJson(string jsonStr)
        {
            JsonResult res;

            try
            {
                res = JsonConvert.DeserializeObject<JsonResult>(jsonStr);
            }
            catch (Exception ex)
            {
                throw new JsonDeserializeException(ex.Message, ex.InnerException);
            }

            return res;
        }

        public void SendEvents(List<SportEventDTO> eventsList)
        {
            ApiConnection apiConnection = new ApiConnection(/*ApiConnectionStrings.URL, ApiConnectionStrings.Controller*/);
            try
            {
                apiConnection.SendEvents(eventsList);
            }
            catch (Exception ex)
            {
                throw new SaveDataException(ex.Message, ex.InnerException);
            }
        }

        public List<SportEventDTO> GetSportEventsList(JsonResult result)
        {
            List<SportEventDTO> eventList = new List<SportEventDTO>();
            List<ResultDTO> resultList;
            string sportType;

            try
            {
                foreach (var ev in result.Events)
                {
                    resultList = new List<ResultDTO>();

                    Sport sport = result.Sports
                                    .Where(n => result.Sections.Where(s => s.Events.Contains(ev.Id))
                                    .FirstOrDefault().Sport == n.Id).FirstOrDefault();

                    if (sport != null)
                    {
                        sportType = sport.Name;

                        if (this.GetTeamsNamesFromEvent(ev, sportType, resultList)
                            && this.AcceptSportType(this.ChangeSportTypeName(Locale.RU, sportType)))
                        {
                            this.GetScoresFromEvent(ev, resultList);

                            SportEventDTO sportEvent = new SportEventDTO();
                            sportEvent.Date = this.GetDateTimeFromSec(ev.StartTime).Ticks;
                            sportEvent.Results = resultList;
                            sportEvent.SportType = this.ChangeSportTypeName(Locale.RU, sportType);

                            eventList.Add(sportEvent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ParseException(ex.Message, ex.InnerException);
            }

            return eventList;
        }

        private void GetScoresFromEvent(Event ev, List<ResultDTO> resultList)
        {
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

            int.TryParse(scores[0], out score);
            resultList[0].Score = score;

            int.TryParse(scores[1], out score);
            resultList[1].Score = score;
        }

        private bool GetTeamsNamesFromEvent(Event ev, string sportType, List<ResultDTO> resultList)
        {
            if (ev.Name.Contains("-") && !ev.Name.Contains(":") && !ev.Name.Contains("1st")
                && !ev.Name.Contains("2nd") && !ev.Name.Contains("1-") && !ev.Name.Contains("2-")
                && !ev.Name.Contains("3-") && ev.Status == 3 && ev.Score.Contains(":"))
            {
                string[] teams = ev.Name.Split(new string[] { "-" }, StringSplitOptions.None);
                for (int i = 0; i < teams.Length; i++)
                {
                    teams[i] = teams[i].Trim(' ');
                }

                resultList.Add(new ResultDTO() { TeamName = teams[0] });
                resultList.Add(new ResultDTO() { TeamName = teams[1] });

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool AcceptSportType(string sportType)
        {
            if (sportType == "Football" || sportType == "Basketball" || sportType == "Hockey")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string ChangeSportTypeName(Locale locale, string name)
        {
            switch (locale)
            {
                case Locale.EN:
                    {
                        return name;
                    }
                case Locale.RU:
                    {
                        if (name == "Футбол")
                        {
                            name = "Football";
                        }
                        if (name == "Баскетбол")
                        {
                            name = "Basketball";
                        }
                        if (name == "Хоккей")
                        {
                            name = "Hockey";
                        }
                        break;
                    }
            }
            return name;
        }

        private DateTime GetDateTimeFromSec(long timeSec)
        {
            int startTime = (int)timeSec;
            int hour, min;
            hour = (startTime / 60 / 60) % 24;
            min = (startTime / 60) % 60;
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, min, 0);
        }
    }
}
