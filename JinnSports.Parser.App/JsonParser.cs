using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JinnSports.Parser.App.JsonEntities;
using JinnSports.DAL.Repositories;
using JinnSports.DAL.Entities;
using JinnSports.DataAccessInterfaces;
using JinnSports.Parser.App.Interfaces;

namespace JinnSports.Parser.App
{
    public class JsonParser:ISaver
    {
        public Uri FonbetUri { get; private set; }

        private EFUnitOfWork uow;

        public JsonParser()
        {
            FonbetUri = new Uri("http://results.fbwebdn.com/results.json.php");
            uow = new EFUnitOfWork();
            
        }

        public string GetJsonFromUrl()
        {
            return this.GetJsonFromUrl(FonbetUri);
        }

        public string GetJsonFromUrl(Uri uri)
        {
            int ch;
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://results.fbwebdn.com/results.json.php");
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            for (int i = 1; ; i++)
            {
                ch = stream.ReadByte();
                if (ch == -1) break;
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

        public void AddDataToDB()
        {
            EFUnitOfWork uow = new EFUnitOfWork();
            IRepository<Team> repo = uow.Teams;
            Team team = new Team();
            team.Name = "Team1";
            repo.Add(team);
        }

        public List<Result> GetResultsList(JsonResult result)
        {
            List<Result> resultList = new List<Result>();
            foreach (var e in result.Events)
            {
                Team team1 = new Team() { Results = new List<Result>() };
                Team team2 = new Team() { Results = new List<Result>() };
                if (GetTeamsFromEvent(e,team1,team2))
                {
                    SportType sportType = new SportType() { Teams = new List<Team>() };
                    Result resTeam1 = new Result();
                    Result resTeam2 = new Result();
                    CompetitionEvent compEvent = new CompetitionEvent() { Date = GetEventDate(e), Results = new List<Result>() };

                    var sports = result.Sections.Where(n => n.Events.Contains(e.Id)).Select(n => n).ToList();
                    sportType.Name = result.Sports.Where(n => n.Id == sports[0].Sport).Select(n => n).ToList()[0].Name;
                    sportType.Teams.Add(team1);
                    sportType.Teams.Add(team2);

                    team1.SportType = sportType;
                    team2.SportType = sportType;

                    GetResultFromEvent(e, resTeam1, team1, false);
                    GetResultFromEvent(e, resTeam2, team2, true);
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

        private DateTime GetEventDate(Event ev)
        {
            int startTime = (int)ev.StartTime;
            int hour, min;
            hour = (startTime / 60 / 60) % 24;
            min = (startTime / 60) % 60;
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, min,0);
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
                res.Score = String.Format("{0}:{1}", scores[1], scores[0]);
            }
        }

        public bool GetTeamsFromEvent(Event ev, Team team1, Team team2)
        {
            if (ev.Name.Contains("-") && !ev.Name.Contains(":") && ev.Status==3)
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
            uow.Results.AddAll(results.ToArray());
        }
    }
}
