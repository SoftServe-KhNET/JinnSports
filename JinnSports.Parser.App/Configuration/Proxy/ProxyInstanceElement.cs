using System.Configuration;

namespace JinnSports.Parser.App.Configuration.Proxy
{
    public class ProxyInstanceElement : ConfigurationElement
    {
        [ConfigurationProperty("profile", IsKey = true, IsRequired = true)]
        public string Profile
        {
            get { return (string)base["profile"]; }
            set { base["profile"] = value; }
        }
        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get { return (string)base["path"]; }
            set { base["path"] = value; }
        }
        [ConfigurationProperty("cooldown", IsRequired = true)]
        public int Cooldown
        {
            get { return (int)base["cooldown"]; }
            set { base["cooldown"] = value; }
        }
        [ConfigurationProperty("asyncinterval", IsRequired = true)]
        public int AsyncInterval
        {
            get { return (int)base["asyncinterval"]; }
            set { base["asyncinterval"] = value; }
        }
        [ConfigurationProperty("timeout", IsRequired = true)]
        public int Timeout
        {
            get { return (int)base["timeout"]; }
            set { base["timeout"] = value; }
        }
    }
}