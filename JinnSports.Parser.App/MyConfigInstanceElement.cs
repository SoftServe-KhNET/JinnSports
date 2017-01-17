using System.Configuration;

namespace JinnSports.Parser.App
{
    public class MyConfigInstanceElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsKey = true, IsRequired = true)]
        public string Path
        {
            get { return (string)base["path"]; }
            set { base["path"] = value; }
        }
    }
}