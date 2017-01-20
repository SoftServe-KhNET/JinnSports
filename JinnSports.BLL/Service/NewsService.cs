using System.Collections.Generic;
using JinnSports.BLL.Dtos;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Utilities;

namespace JinnSports.BLL.Service
{
    public class NewsService : INewsService
    {
        public ICollection<NewsDto> GetLastNews()
        {
            int newsCount = NewsConfiguration.Configuration.MaxNumberOfNews;

            foreach (NewsConfiguration.NewsSourcesElement source in NewsConfiguration.Configuration.Sources)
            {
                if (!string.IsNullOrEmpty(source.Link))
                {
                    string url = source.Link;
                    var news = RssParser.RssParser.ParseNews(url, newsCount);

                    if (news != null)
                    {
                        return news;
                    }
                }
            }

            return null;
        }
    }
}