using JinnSports.Parser.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.Parser.App.WebConnection
{
    public class ProxyHttpWebResponse : IProxyHttpWebResponse
    {
        public string Proxy { get; set; }
        public HttpWebResponse Response { get; set; }
    }
}
