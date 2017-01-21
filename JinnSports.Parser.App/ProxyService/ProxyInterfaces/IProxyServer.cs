using JinnSports.Parser.App.ProxyService.ProxyEnums;
using System;

namespace JinnSports.Parser.App.ProxyService.ProxyInterfaces
{
    public interface IProxyServer
    {
        string Ip { get; set; }
        ProxyStatus Status { get; set; }
        int Priority { get; set; }
        DateTime LastUsed { get; set; }
        bool IsBusy { get; set; }
    }
}
