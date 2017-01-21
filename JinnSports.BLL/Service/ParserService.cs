using JinnSports.DAL.EFContext;
using JinnSports.DAL.Repositories;
using JinnSports.Parser.App.Configuration.Parser;
using JinnSports.Parser.App.HtmlParsers;
using JinnSports.Parser.App.JsonParsers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JinnSports.BLL.Service
{
    public static class ParserService
    {
        private static CancellationToken cancellationToken = new CancellationToken();

        public static void Initialize()
        {
            int timeInterval = ParserSettings.GetInterval("original");

            Task t = Task.Factory.StartNew(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Run();
                    Task.Delay(timeInterval * 1000, cancellationToken);
                }
            });
        }

        public static void Run()
        {

            JsonParser jsonParser = new JsonParser();
            HTMLParser24score htmlParser = new HTMLParser24score(new EFUnitOfWork(new SportsContext("SportsContext")));

            List<Task> tasks = new List<Task>();
            /*tasks.Add(Task.Factory.StartNew(() =>
            {
                jsonParser.StartParser();
            }));*/

            tasks.Add(Task.Factory.StartNew(() =>
            {
                htmlParser.Parse();
            }));

            Task.WaitAll(tasks.ToArray());
        }
    }
}
