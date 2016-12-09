using System;

namespace JinnSports.Parser.App.ProxyService.ProxyInterfaces
{
    public interface IProxyServer
    {
        string Ip { get; set; }
        string Status { get; set; }
        int Priority { get; set; }
        DateTime LastUsed { get; set; }
    }
}
