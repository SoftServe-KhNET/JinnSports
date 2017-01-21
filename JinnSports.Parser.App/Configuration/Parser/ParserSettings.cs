using System.Configuration;

namespace JinnSports.Parser.App.Configuration.Parser
{
    public static class ParserSettings
    {
        public static int GetInterval(string name)
        {
            var config = ConfigurationManager.GetSection("ParserSettings") as ParserSection;
            return config.Instances[name].Interval;
        }
    }
}