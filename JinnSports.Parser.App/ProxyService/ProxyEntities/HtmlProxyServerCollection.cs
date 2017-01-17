using System.Collections.Generic;

namespace JinnSports.Parser.App.ProxyService.ProxyEntities
{
    public class HtmlProxyServerCollection
    {
        public HtmlProxyServerCollection()
        {
            this.HtmlProxies = new List<HtmlProxyServer>();
        }
        public List<HtmlProxyServer> HtmlProxies { get; set; }
    }
}