using System.Configuration;

namespace JinnSports.Parser.App.Configuration.Parser
{
    public class ParserInstanceElement : ConfigurationElement
    {
        [ConfigurationProperty("profile", IsKey = true, IsRequired = true)]
        public string Profile
        {
            get { return (string)base["profile"]; }
            set { base["profile"] = value; }
        }
          
        [ConfigurationProperty("interval", IsRequired = true)]
        public int Interval
        {
            get { return (int)base["interval"]; }
            set { base["interval"] = value; }
        }
    }
}