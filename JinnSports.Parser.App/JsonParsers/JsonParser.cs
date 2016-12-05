﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using JinnSports.Entities;
using JinnSports.DAL.Repositories;
using JinnSports.Parser.App.Interfaces;
using JinnSports.Parser.App.JsonParsers.JsonEntities;
using JinnSports.Parser.App.ProxyService.ProxyParser;
using JinnSports.Parser.App.ProxyService.ProxyConnection;
using log4net;
using log4net.Config;

namespace JinnSports.Parser.App.JsonParsers
{
    public class JsonParser : ISaver
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(JsonParser));

        private EFUnitOfWork uow;

        public JsonParser()
        {
            this.FonbetUri = new Uri("http://results.fbwebdn.com/results.json.php");
            this.uow = new EFUnitOfWork("SportsContext");
            XmlConfigurator.Configure();
        }
        public Uri FonbetUri { get; private set; }

        public void StartJsonParser()
        {
            JsonResult jResults = this.DeserializeJson(this.GetJsonFromUrl(this.FonbetUri));
            List<Result> res = this.GetResultsList(jResults);
            this.DBSaveChanges(res);
        }

        public string GetJsonFromUrl()
        {
            return this.GetJsonFromUrl(this.FonbetUri);
        }

        public string GetJsonFromUrl(Uri uri)
        {
            return this.GetJsonFromUrl(this.FonbetUri,null);
        }

        public string GetJsonFromUrl(Uri uri, string proxy)
        {
            ProxyConnection pc = new ProxyConnection();
            while (true)
            {
                proxy = pc.GetProxy();
                if (pc.CanPing(proxy) == true)
                {
                    try
                    {
                        int ch;
                        string result = string.Empty;
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
                        WebProxy webProxy = new WebProxy(proxy, true);
                        req.Proxy = webProxy;
                        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
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
                        log.Info("Json was read from site");
                        return result;
                    }
                    catch (Exception e)
                    {
                        pc.SetStatus(proxy, false);
                    }
                }
                else
                {
                    pc.SetStatus(proxy, false);
                }
            }
        }

        public JsonResult DeserializeJson(string jsonStr)
        {
            JsonResult res = JsonConvert.DeserializeObject<JsonResult>(jsonStr);
            log.Info("Json data was deserialized");
            return res;
        }

        public List<Result> GetResultsList(JsonResult result)
        {
            List<Result> resultList = new List<Result>();
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
                    sportType.Name = result.Sports.Where(n => n.Id == sports[0].Sport).Select(n => n).ToList()[0].Name;
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

        public void GetResultFromEvent(Event ev, Result res, Team team, bool invertScore)
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

            if (!invertScore)
            {
                res.Score = mainScore;
            }
            else
            {
                string[] scores = mainScore.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                res.Score = string.Format("{0}:{1}", scores[1], scores[0]);
            }
        }

        public bool GetTeamsFromEvent(Event ev, Team team1, Team team2)
        {
            if (ev.Name.Contains("-") && !ev.Name.Contains(":") && ev.Status == 3)
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

        public void DBSaveChanges(List<Result> results)
        {
            uow.Set<Result>().AddAll(results.ToArray());
            uow.SaveChanges();
            log.Info(String.Format("Results for {0}.{1}.{2} was added to DB", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year));
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
