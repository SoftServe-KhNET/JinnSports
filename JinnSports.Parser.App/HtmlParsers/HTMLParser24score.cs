using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Parser.App.Exceptions;
using JinnSports.Parser.App.ProxyService.ProxyConnection;
using JinnSports.Entities.Entities;
using log4net;

namespace JinnSports.Parser.App.HtmlParsers
{
    public class HTMLParser24score
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HTMLParser24score(IUnitOfWork unit)
        {
            this.Unit = unit;
        }

        public IUnitOfWork Unit { get; private set; }
        public uint DaysCount { get; set; }

        public void Parse(uint daysCount = 1)
        {
            Log.Info("Html parser was started");
            try
            {
                Uri footballUrl = new Uri("https://24score.com/?date=");
                Uri basketballUrl = new Uri("https://24score.com/basketball/?date=");
                Uri hokkeyUrl = new Uri("https://24score.com/ice_hockey/?date=");
                List<Uri> selectedUris = new List<Uri> { footballUrl, basketballUrl, hokkeyUrl };

                foreach (Uri baseUrl in selectedUris)
                {
                    string currentSport;
                    if (baseUrl.OriginalString.Contains("basketball"))
                    {
                        currentSport = "Basketball";
                    }
                    else if (baseUrl.OriginalString.Contains("ice_hockey"))
                    {
                        currentSport = "Hockey";
                    }
                    else
                    {
                        currentSport = "Football";
                    }

                    SportType sport;
                    try
                    {
                        sport = this.Unit.GetRepository<SportType>().Get(t => t.Name == currentSport).FirstOrDefault();
                    }
                    catch(Exception ex)
                    {
                        throw new GetDataException(ex.Message, ex.InnerException);
                    }
                    DateTime now = DateTime.Now;

                    for (int i = 1; i < daysCount + 1; i++)
                    {
                        DateTime date = now.Subtract(new TimeSpan(i, 0, 0, 0));
                        string url = baseUrl.ToString() + date.Date.ToString("yyyy-MM-dd");
                        string html = this.GetHtml(url);
                        List<Result> results;

                        try
                        {
                            results = this.ParseHtml(html, sport, date);
                        }
                        catch (Exception ex)
                        {
                            throw new ParseException(ex.Message, ex.InnerException);
                        }

                        try
                        {
                            this.PushEntities(results);
                            Log.Info("New data from html parser was saved to DataBase");
                        }
                        catch (Exception ex)
                        {
                            throw new SaveDataException(ex.Message, ex.InnerException);
                        }
                    }
                }
                this.Unit.Dispose();
            }
            catch (GetDataException ex)
            {
                Log.Error(ex);
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

        private string GetHtml(string url)
        {
            ProxyConnection pc = new ProxyConnection();
            HttpWebResponse resp;

            WebRequest reqGet = WebRequest.Create(url);
            reqGet.Headers.Set(HttpRequestHeader.ContentEncoding, "1251");

            try
            {
                resp = pc.MakeProxyRequest(url, 0);
                if (resp == null)
                {
                    resp = (HttpWebResponse)reqGet.GetResponse();
                }
            }
            catch(Exception ex)
            {
                throw new WebResponseException(ex.Message, ex.InnerException);
            }

            string html = new StreamReader(resp.GetResponseStream()).ReadToEnd();

            return html;
        }

        private List<Result> ParseHtml(string html, SportType sportType, DateTime date)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(html);
            List<Result> results = new List<Result>();

            foreach (IElement htmlTr in document.QuerySelectorAll(".daymatches tr[class]:not(.hidden)"))
            {
                SportEvent c = new SportEvent() { Date = date };

                string name1 = htmlTr.Children[1].FirstElementChild.TextContent;
                string name2 = htmlTr.Children[2].FirstElementChild.TextContent;

                Team t1 = this.Unit.GetRepository<Team>().Get(t => t.Name == name1).FirstOrDefault();
                if (t1 == null)
                {
                    t1 = new Team { Name = name1, SportType = sportType };
                }

                Team t2 = this.Unit.GetRepository<Team>().Get(t => t.Name == name2).FirstOrDefault();
                if (t2 == null)
                {
                    t2 = new Team { Name = name2, SportType = sportType };
                }

                var score = htmlTr.QuerySelectorAll("span[id*='score']")[0].TextContent;
                score = score.Replace("\n", string.Empty).Replace("\t", string.Empty);
                score = score.Split('(')[0];

                int score1;
                int score2;

                Result result1 = new Result { Team = t1, SportEvent = c };
                Result result2 = new Result { Team = t2, SportEvent = c };

                if (score.Contains(":"))
                {
                    var scores = score.Split(':');
                    bool sc1 = int.TryParse(scores[0], out score1);
                    bool sc2 = int.TryParse(scores[1], out score2);

                    result1.Score = score1;
                    result2.Score = score2;                
                }
                else
                {
                    continue;
                }

                results.Add(result1);
                results.Add(result2);
            }

            return results;
        }

        private void PushEntities(List<Result> results)
        {
            foreach (Result item in results)
            {
                this.Unit.GetRepository<Result>().Insert(item);
            }

            this.Unit.SaveChanges();
        }
    }
}