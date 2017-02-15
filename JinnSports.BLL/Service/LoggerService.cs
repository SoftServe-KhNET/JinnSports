using JinnSports.BLL.Dtos.ClientLog;
using JinnSports.BLL.Interfaces;
using log4net;
using System;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace JinnSports.BLL.Service
{
    public class LoggerService : ILoggerService
    {
        private static readonly ILog log = LogManager.GetLogger("ClientLog");

        public void SaveLog(LogDto logs)
        {
            foreach(LogEventDto logEvent in logs.Events)
            {
                log.Info(String.Format("{0}\t{1} {2}#{3} value=\"{4}\" X={5} Y={6}", 
                    UnixTimeStampToDateTime(logEvent.Time), logEvent.Event, logEvent.TagName, 
                    logEvent.Id, logEvent.Value.Length < 15 ? logEvent.Value:"", logEvent.CoordX, 
                    logEvent.CoordY));
            }
        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
