using JinnSports.Parser.App.ProxyService.ProxyInterfaces;

namespace JinnSports.Parser.App.ProxyService.ProxyEntities
{
    public class HtmlProxyServer : IHtmlProxyServer
    {
        public string Ip { get; set; }
        public string Port { get; set; }
        public string Anonymity { get; set; }
        public string Type { get; set; }
        public double Ping { get; set; }
    }
}
