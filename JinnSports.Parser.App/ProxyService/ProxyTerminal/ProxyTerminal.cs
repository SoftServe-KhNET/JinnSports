using JinnSports.Parser.App.ProxyService.ProxyInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JinnSports.Parser.App.ProxyService.ProxyConnections;
using JinnSports.Parser.App.Exceptions;
using System.Diagnostics;
using System.Threading;
using JinnSports.Parser.App.ProxyService.ProxyEnums;

namespace JinnSports.Parser.App.ProxyService.ProxyTerminal
{
    public class ProxyTerminal : IProxyTerminal
    {
        private IProxyAsync proxyAsync;
        private ProxyConnection pc;
  
        public ProxyTerminal()
        {
            this.pc = new ProxyConnection();
        }

        public void MakeProxyUnavaliable(string proxy)
        {
            pc.SetStatus(proxy, ConnectionStatus.CS_СonnectedWrongly);
        }

        public HttpWebResponse GetProxyResponse(Uri url)
        {
            this.proxyAsync = new ProxyAsync(this.pc, url);
            return proxyAsync.GetProxyAsync();
        }
    }
}
