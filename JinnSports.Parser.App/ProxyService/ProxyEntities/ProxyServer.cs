using System;
using System.Xml.Serialization;
using JinnSports.Parser.App.ProxyService.ProxyInterfaces;
using JinnSports.Parser.App.ProxyService.ProxyEnums;

namespace JinnSports.Parser.App.ProxyService.ProxyEntities
{
    [Serializable]
    public class ProxyServer : BaseProxyServer, IProxyServer
    {
        [XmlAttribute]
        public ProxyStatus Status { get; set; }
        [XmlAttribute]
        public int Priority { get; set; }
        [XmlElement]
        public DateTime LastUsed { get; set; }
        [XmlElement]
        public bool IsBusy { get; set; }
    }
}
