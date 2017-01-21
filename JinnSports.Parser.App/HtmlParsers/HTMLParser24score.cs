using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using JinnSports.Parser.App.Exceptions;
using JinnSports.Parser.App.ProxyService.ProxyConnections;
using log4net;
using DTO.JSON;
using JinnSports.Parser.App.ProxyService.ProxyTerminal;
using JinnSports.Parser.App.ProxyService.ProxyInterfaces;
using JinnSports.DataAccessInterfaces.Interfaces;

namespace JinnSports.Parser.App.HtmlParsers
{
    public class HTMLParser24score
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IProxyTerminal proxyTerminal;

        public HTMLParser24score()
        {
            proxyTerminal = new ProxyTerminal();
        }
        public uint DaysCount { get; set; }

        public void Parse(uint daysCount = 1)
        {
            Log.Info("Html parser was started");
            try
            {
                ApiConnection api = new ApiConnection(/*ApiConnectionStrings.URL, ApiConnectionStrings.Controller*/);

                Uri footballUrl = new Uri("https://24score.com/?date=");
                //Uri basketballUrl = new Uri("https://24score.com/basketball/?date=");
                //Uri hokkeyUrl = new Uri("https://24score.com/ice_hockey/?date=");
                List<Uri> selectedUris = new List<Uri> { footballUrl/*, basketballUrl, hokkeyUrl*/ };
                
                foreach (Uri baseUrl in selectedUris)                
                {
                    string currentSport;
                    if (baseUrl.OriginalString.ToUpper().Contains("BASKETBALL"))
                    {
                        currentSport = "Basketball";
                    }
                    else if (baseUrl.OriginalString.ToUpper().Contains("HOCKEY"))
                    {
                        currentSport = "Hockey";
                    }
                    else
                    {
                        currentSport = "Football";
                    }                    
                    
                    DateTime now = DateTime.Now;

                    for (int i = 1; i < daysCount + 1; i++)
                    {
                        DateTime dateTime = now.Subtract(new TimeSpan(i, 0, 0, 0));
                        long date = dateTime.Ticks;
                        string url = baseUrl.ToString() + dateTime.Date.ToString("yyyy-MM-dd");
                        string html = this.GetHtml(url);
                        List<SportEventDTO> events = this.ParseHtml(html, currentSport, date);

                        try
                        {
                            api.SendEvents(events);
                        }
                        catch (Exception ex)
                        {
                            throw new SaveDataException(ex.Message, ex.InnerException);
                        }
                    }
                }
                Log.Info("New data from HTML parser was sent");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }            
        }

        private string GetHtml(string url)
        {
            ProxyConnection pc = new ProxyConnection();
            HttpWebResponse response;

            WebRequest reqGet = WebRequest.Create(url);
            reqGet.Headers.Set(HttpRequestHeader.ContentEncoding, "1251");

            try
            {
                response = this.proxyTerminal.GetProxyResponse(new Uri(url));
            }
            catch (Exception ex)
            {
                throw new WebResponseException(ex.Message, ex.InnerException);
            }

            string html = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return html;
            //return "";
        }

        private List<SportEventDTO> ParseHtml(string html, string currentSport, long date)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(html);
            List<SportEventDTO> events = new List<SportEventDTO>();

            try
            {
                foreach (IElement htmlTr in document.QuerySelectorAll(".daymatches tr[class]:not(.hidden)"))
                {
                    SportEventDTO currentEvent = new SportEventDTO() { Date = date, SportType = currentSport };

                    string teamName1 = htmlTr.Children[1].FirstElementChild.TextContent;
                    string teamName2 = htmlTr.Children[2].FirstElementChild.TextContent;

                    var score = htmlTr.QuerySelectorAll("span[id*='score']")[0].TextContent;
                    score = score.Replace("\n", string.Empty).Replace("\t", string.Empty);
                    score = score.Split('(')[0];

                    int score1;
                    int score2;

                    ResultDTO result1 = new ResultDTO { TeamName = teamName1 };
                    ResultDTO result2 = new ResultDTO { TeamName = teamName2 };

                    if (score.Contains(":"))
                    {
                        var scores = score.Split(':');
                        int.TryParse(scores[0], out score1);
                        int.TryParse(scores[1], out score2);

                        result1.Score = score1;
                        result2.Score = score2;
                    }
                    else if (score.Contains("— —"))
                    {
                        result1.Score = null;
                        result2.Score = null;
                    }
                    else
                    {
                        continue;
                    }

                    currentEvent.Results.Add(result1);
                    currentEvent.Results.Add(result2);

                    events.Add(currentEvent);
                }
            }
            catch (Exception ex)
            {
                throw new ParseException(ex.Message, ex.InnerException);
            }

            return events;
        }        
    }
}