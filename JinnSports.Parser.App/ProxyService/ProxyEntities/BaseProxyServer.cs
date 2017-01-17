using System.Xml.Serialization;

namespace JinnSports.Parser.App.ProxyService.ProxyEntities
{
    public abstract class BaseProxyServer
    {
        [XmlElement]
        public string Ip { get; set; }
    }
}