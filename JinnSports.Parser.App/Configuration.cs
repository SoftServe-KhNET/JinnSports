using System.Configuration;

namespace JinnSports.Parser.App
{
    public static class ConfigSettings
    {
        public static string Xml()
        {
            var config = ConfigurationManager.GetSection("ProxyXml") as MyConfigSection;
            return config.Instances[0].Path;
        }
    }
}