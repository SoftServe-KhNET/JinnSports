using System.Configuration;

namespace JinnSports.Parser.App.Configuration.Proxy
{
    public static class ProxySettings
    {
        public static string GetPath(string name)
        {
            var config = ConfigurationManager.GetSection("ProxySettings") as ProxySection;
            return config.Instances[name].Path;
        }

        public static int GetAsyncInterval(string name)
        {
            var config = ConfigurationManager.GetSection("ProxySettings") as ProxySection;
            return config.Instances[name].AsyncInterval;
        }

        public static int GetCooldown(string name)
        {
            var config = ConfigurationManager.GetSection("ProxySettings") as ProxySection;
            return config.Instances[name].Cooldown;
        }

        public static int GetTimeout(string name)
        {
            var config = ConfigurationManager.GetSection("ProxySettings") as ProxySection;
            return config.Instances[name].Timeout;
        }
    }
}