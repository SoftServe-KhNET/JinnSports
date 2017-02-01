using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.Parser.App.Interfaces
{
    public interface IProxyHttpWebResponse
    {
        string Proxy { get; set; }
        HttpWebResponse Response { get; set; }
    }
}
