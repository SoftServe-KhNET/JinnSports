using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml;

namespace PredictorApplication.Models.Settings
{
    public class SettingsReader
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SettingsReader));

        private static SettingsReader instance;
        private static readonly int DEFAULT_SCORE = 5;
        private readonly object lockMonitor = new object();

        private SettingsReader()
        {

        }

        public static SettingsReader GetInstance()
        {
            if (instance == null)
            {
                instance = new SettingsReader();
            }
            return instance;
        }

        public int ReadMaxScore(string type)
        {
            lock (lockMonitor)
            {
                int maxScore = DEFAULT_SCORE;

                try
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(HostingEnvironment.MapPath("~/App_Data/") + "sport-types.xml");
                    XmlElement xNodes = xDoc.DocumentElement;

                    foreach (XmlNode xNode in xNodes)
                    {
                        if (xNode.Attributes.Count > 0)
                        {
                            XmlNode attr = xNode.Attributes.GetNamedItem("type");
                            if ((attr != null) && (attr.Value == type))
                            {
                                int.TryParse(xNode.InnerText, out maxScore);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Exception while trying to read SportTypes", ex);
                }

                return maxScore;
            }
        }
    }
}