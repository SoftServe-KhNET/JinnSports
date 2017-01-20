using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using JinnSports.BLL.Dtos;

namespace JinnSports.BLL.RssParser
{
    public static class RssParser
    {
        /// <summary>
        /// Get list of news from resource
        /// </summary>
        /// <param name="url">resource URL</param>
        /// <param name="newsCount">news to take</param>
        /// <returns>list of news</returns>
        public static ICollection<NewsDto> ParseNews(string url, int newsCount)
        {
            string result;
            
            try
            {
                result = GetFeed(url);
            }
            catch (WebException)
            {
                return null;
            }

            var news = GetNewsFromXml(result);

            return news.Take(newsCount).ToList();
        }

        /// <summary>
        /// Parse XML string to list of news
        /// </summary>
        /// <param name="result">string with XML</param>
        /// <returns>list of news</returns>
        private static ICollection<NewsDto> GetNewsFromXml(string result)
        {
            XDocument document = XDocument.Parse(result);

            return (from descendant in document.Descendants("item")
                    select new NewsDto
                    {
                        Time = DateTime.Parse(descendant.Element("pubDate")?.Value)
                            .ToShortTimeString(),
                        Title = descendant.Element("title")?.Value,
                        Description = descendant.Element("description")?.Value,
                        Link = descendant.Element("link")?.Value
                    })
                .ToList();
        }

        /// <summary>
        /// Get XML string from resource URL
        /// </summary>
        /// <param name="url">resource URL</param>
        /// <returns>XML result</returns>
        private static string GetFeed(string url)
        {
            var webClient = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            string result = webClient.DownloadString(url);

            return result;
        }
    }
}