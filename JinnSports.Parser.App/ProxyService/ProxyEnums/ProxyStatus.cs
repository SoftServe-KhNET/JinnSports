using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.Parser.App.ProxyService.ProxyEnums
{
    public enum ProxyStatus
    {
        PS_New,
        PS_Stable,
        PS_Unstable,
        PS_Bad,
        PS_Eliminated
    }
    public enum ConnectionStatus
    {
        CS_Connected,
        CS_Disconnected,
        CS_PreResponseTerminated,
        CS_PostResponseTerminated
    }
}
