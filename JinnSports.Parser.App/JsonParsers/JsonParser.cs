﻿using DTO.JSON;
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
using JinnSports.Parser.App.WebConnection;

namespace JinnSports.Parser.App.JsonParsers
{
    public enum Locale
    {
        EN, RU
    }

    public enum EventStatus
    {
        NotStarted = 0, InPlay = 2, Finished = 3
    }

    public class JsonParser
    {
        private static readonly ILog Log = LogManager.GetLogger("AppLog");
        private IProxyTerminal proxyTerminal;

        public JsonParser() : this(new Uri("http://results.fbwebdn.com/results.json.php"))
        {

        }

        public JsonParser(Uri uri)
        {
            this.proxyTerminal = new ProxyTerminal();
            this.SiteUri = uri;
        }

        public Uri SiteUri { get; private set; }

        public void StartParser()
        {
            Log.Info("Json parser was started");
            try
            {
                while (true)
                {
                    string proxy = string.Empty;
                    try
                    {
                        string result = this.GetJsonFromUrl(this.SiteUri, out proxy);
                        JsonResult jsonResults = this.DeserializeJson(result);
                        List<SportEventDTO> sportEventsList = this.GetSportEventsList(jsonResults);
                        this.SendEvents(sportEventsList);
                        break;
                    }
                    catch (Exception ex)
                    {
                        this.proxyTerminal.MakeProxyUnavaliable(proxy);
                        Log.Error(ex);
                    }
                }
                Log.Info("New data from JSON parser was sent");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        public string GetJsonFromUrl()
        {
            string proxy = string.Empty;
            return this.GetJsonFromUrl(this.SiteUri, out proxy);
        }

        public string GetJsonFromUrl(Uri uri, out string proxy, Locale locale = Locale.RU)
        {
            string result = string.Empty;
            ProxyHttpWebResponse response;

            string url = string.Format("{0}?locale={1}", uri.ToString(), locale == Locale.EN ? "en" : "ru");

            response = this.proxyTerminal.GetProxyResponse(new Uri(url));
            result = new StreamReader(response.Response.GetResponseStream()).ReadToEnd();
            proxy = response.Proxy;
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
            ApiConnection apiConnection = new ApiConnection();
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
                            if (ev.Status == (int)EventStatus.Finished)
                            {
                                this.GetScoresFromEvent(ev, resultList);
                            }

                            SportEventDTO sportEvent = new SportEventDTO();
                            sportEvent.Date = this.UnixToDateTime(ev.StartTime).Ticks;
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

        public DateTime UnixToDateTime(long unixTime)
        {
            DateTime eventTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            eventTime = eventTime.AddSeconds(unixTime);
            return eventTime;
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
                && !ev.Name.Contains("3-") && ((ev.Status == (int)EventStatus.Finished && ev.Score.Contains(":"))
                || ev.Status == (int)EventStatus.NotStarted))
            {
                string[] teams = ev.Name.Split(new string[] { "-" }, StringSplitOptions.None);
                for (int i = 0; i < teams.Length; i++)
                {
                    teams[i] = teams[i].Trim(' ');
                }

                resultList.Add(new ResultDTO() { TeamName = teams[0], IsHome = true });
                resultList.Add(new ResultDTO() { TeamName = teams[1], IsHome = false });

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
    }
}