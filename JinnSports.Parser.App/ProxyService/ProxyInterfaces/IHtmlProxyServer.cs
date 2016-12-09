namespace JinnSports.Parser.App.ProxyService.ProxyInterfaces
{
    public interface IHtmlProxyServer
    {
        string Ip { get; set; }
        string Port { get; set; }
        string Anonymity { get; set; }
        string Type { get; set; }
        double Ping { get; set; }
    }
}
