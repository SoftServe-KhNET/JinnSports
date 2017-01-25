using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace PredictorApplication.Models.Settings
{
    public class SettingsReader
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SettingsReader));

        public int ReadMaxScore(string type)
        {
            int maxScore = 5;
            /*
            try
            {
                XmlDocument xDoc = new XmlDocument();
                //TODO: resolve loading from IIS directory
                xDoc.Load(@"C:\HomeControlTest\sport-types.xml");
                XmlElement xNodes = xDoc.DocumentElement;

                foreach (XmlNode xNode in xNodes)
                {
                    if (xNode.Attributes.Count > 0)
                    {
                        XmlNode attr = xNode.Attributes.GetNamedItem("type");
                        if ((attr != null) && (attr.Value == type))
                        {
                            int.TryParse(xNode.SelectSingleNode("max-score").InnerText, out maxScore);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Exception while trying to read SportTypes", ex);
            }
            */

            return maxScore;
        }
    }
}