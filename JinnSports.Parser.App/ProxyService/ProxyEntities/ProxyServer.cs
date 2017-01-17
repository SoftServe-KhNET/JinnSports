using System;
using System.Xml.Serialization;
using JinnSports.Parser.App.ProxyService.ProxyInterfaces;

namespace JinnSports.Parser.App.ProxyService.ProxyEntities
{
    [Serializable]
    public class ProxyServer : BaseProxyServer, IProxyServer
    {
        [XmlAttribute]
        public string Status { get; set; }
        [XmlAttribute]
        public int Priority { get; set; }
        [XmlElement]
        public DateTime LastUsed { get; set; }
    }
}
