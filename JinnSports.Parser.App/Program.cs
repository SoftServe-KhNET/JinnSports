using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JinnSports.Parser.App.JsonEntities;

namespace JinnSports.Parser.App
{
    class Program
    {
        static void Main(string[] args)
        {
            JsonParser jp = new JsonParser();
            Result res = jp.DeserializeJsonString(jp.GetJsonFromUrl());
            foreach(var section in res.Sections)
            {
                Console.WriteLine("\n\t\t{0}",section.Name);
                var eventList = res.Events.Where(n => section.Events.Contains(n.Id));
                foreach (var e in eventList)
                {
                    Console.WriteLine("{0}\t {1} {2} {3} {4} {5}", 
                        ConvertTime(e.StartTime), e.Name, e.Score, e.Comment1, e.Comment2, e.Comment3);
                }
            }
            Console.ReadKey();
        }

        static string ConvertTime(long time)
        {
            long hour, min;
            hour = (time / 60 / 60) % 24+TimeZoneInfo.Local.BaseUtcOffset.Hours;
            min = (time / 60) % 60;
            string format = (hour < 10 ? "0" : "") + "{0}:" + (min < 10 ? "0" : "") + "{1}";
            return String.Format(format, hour, min);
        }
    }
}
