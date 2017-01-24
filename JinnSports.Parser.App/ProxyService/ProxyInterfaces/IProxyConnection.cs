using JinnSports.Parser.App.ProxyService.ProxyEnums;
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

        HttpWebResponse GetProxyResponse(Uri url, int timeout, CancellationToken token, bool asyncResponse);

        void UpdateElimination();
    }
}
