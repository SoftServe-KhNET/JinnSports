using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace PredictorApplication.Models.Settings
{
    public class SettingsReader
    {
        public int ReadMaxScore(string type)
        {
            int maxScore = 5;

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("sport-types.xml");
            XmlElement xNodes = xDoc.DocumentElement;

            foreach (XmlNode xNode in xNodes)
            {
                if (xNode.Attributes.Count > 0)
                {
                    XmlNode attr = xNode.Attributes.GetNamedItem("type");
                    if ((attr != null) && (attr.Value == type))
                    {
                        Int32.TryParse(xNode.SelectSingleNode("max-score").InnerText, out maxScore);
                    }
                }
            }

            return maxScore;
        }
    }
}