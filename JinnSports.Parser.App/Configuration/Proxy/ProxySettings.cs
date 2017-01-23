using System.Configuration;

namespace JinnSports.Parser.App.Configuration.Proxy
{
    public static class ProxySettings
    {
        public static string GetPath()
        {
            var config = ConfigurationManager.GetSection("ProxySettings") as ProxySection;
            return config.Instances[ConfigurationManager.AppSettings.Get("currentProxyProfile")].Path;
        }

        public static int GetAsyncInterval()
        {
            var config = ConfigurationManager.GetSection("ProxySettings") as ProxySection;
            return config.Instances[ConfigurationManager.AppSettings.Get("currentProxyProfile")].AsyncInterval;
        }

        public static int GetCooldown()
        {
            var config = ConfigurationManager.GetSection("ProxySettings") as ProxySection;
            return config.Instances[ConfigurationManager.AppSettings.Get("currentProxyProfile")].Cooldown;
        }

        public static int GetTimeout()
        {
            var config = ConfigurationManager.GetSection("ProxySettings") as ProxySection;
            return config.Instances[ConfigurationManager.AppSettings.Get("currentProxyProfile")].Timeout;
        }
    }
}