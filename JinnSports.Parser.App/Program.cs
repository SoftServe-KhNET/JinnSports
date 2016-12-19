using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using JinnSports.DAL.Repositories;
using JinnSports.Entities;
using JinnSports.Parser.App.HtmlParsers;
using JinnSports.Parser.App.JsonParsers;
using JinnSports.Parser.App.JsonParsers.JsonEntities;
using JinnSports.Parser.App.ProxyService.ProxyParser;
using JinnSports.Parser.App.ProxyService.ProxyConnection;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using System.Net;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace JinnSports.Parser.App
{
    public class Program
    {
        public static void Main(string[] args)
        {

        }
    }
}
