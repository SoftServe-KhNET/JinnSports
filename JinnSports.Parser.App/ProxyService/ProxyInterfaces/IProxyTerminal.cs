using JinnSports.Parser.App.WebConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.Parser.App.ProxyService.ProxyInterfaces
{
    public interface IProxyTerminal
    {
        ProxyHttpWebResponse GetProxyResponse(Uri url);
        void MakeProxyUnavaliable(string proxy);
    }
}
