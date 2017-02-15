using JinnSports.Parser.App.ProxyService.ProxyEnums;
using JinnSports.Parser.App.WebConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JinnSports.Parser.App.ProxyService.ProxyInterfaces
{
    public interface IProxyConnection
    {
        void SetStatus(string ip, ConnectionStatus status);

        string GetProxy();

        bool CanPing(string address);

        ProxyHttpWebResponse GetProxyResponse(Uri url, int timeout, CancellationToken token, bool asyncResponse);

        void UpdateElimination();
    }
}
