using JinnSports.Parser.App.ProxyService.ProxyInterfaces;
using System;
using System.Net;
using JinnSports.Parser.App.ProxyService.ProxyConnections;
using JinnSports.Parser.App.ProxyService.ProxyEnums;
using JinnSports.Parser.App.WebConnection;

namespace JinnSports.Parser.App.ProxyService.ProxyTerminal
{
    public class ProxyTerminal : IProxyTerminal
    {
        private IProxyAsync proxyAsync;
        private IProxyConnection pc;
  
        public ProxyTerminal()
        {
            this.pc = new ProxyConnection();
        }

        public void MakeProxyUnavaliable(string proxy)
        {
            pc.SetStatus(proxy, ConnectionStatus.CS_СonnectedWrongly);
        }

        public ProxyHttpWebResponse GetProxyResponse(Uri url)
        {
            this.proxyAsync = new ProxyAsync(this.pc, url);
            return proxyAsync.GetProxyAsync();
        }
    }
}
