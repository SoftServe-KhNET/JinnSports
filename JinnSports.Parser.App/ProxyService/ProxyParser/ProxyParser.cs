using System;
using System.Linq;
using JinnSports.Parser.App.ProxyService.ProxyRepository;
using System.Net;
using System.IO;
using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using System.Globalization;
using System.Collections.Generic;
using JinnSports.Parser.App.ProxyService.ProxyEntities;
using JinnSports.Parser.App.ProxyService.ProxyEnums;
using JinnSports.Parser.App.Configuration.Proxy;
using System.Configuration;
using System.Web;
using System.Web.Hosting;

namespace JinnSports.Parser.App.ProxyService.ProxyParser
{
    public class ProxyParser
    {
        private string path;

        public ProxyParser()
        {
            this.path = HostingEnvironment.MapPath("~/App_Data/") + ProxySettings.GetPath();
        }

        public void UpdateData(bool clearData, string url)
        {
            ProxyRepository<ProxyServer> writer = new ProxyRepository<ProxyServer>();
            if (clearData)
            {
                writer.Clear();
            }
            this.UpdateData(url);
        }

        public void UpdateData(string url)
        {
            HtmlProxyServerCollection service_proxies = this.GetProxiesFromService(url);
            this.SaveProxiesToXml(service_proxies);
        }

        public void UpdateData()
        {
            this.UpdateData("http://foxtools.ru/Proxy");
        }

        private void SaveProxiesToXml(HtmlProxyServerCollection service_proxies)
        {
            ProxyRepository<ProxyServer> xmlWriter = new ProxyRepository<ProxyServer>();
            if (!File.Exists(this.path))
            {
                xmlWriter.Clear();
            }
            List<ProxyServer> proxyCollection = new List<ProxyServer>();
            DateTime defaultLastUsed = DateTime.Now.AddMinutes(-xmlWriter.Interval);
            foreach (HtmlProxyServer service_proxy in service_proxies.HtmlProxies)
            {
                ProxyServer proxy = new ProxyServer();
                if (!xmlWriter.Contains(service_proxy.Ip))
                {
                    if (service_proxy.Ping < 15)
                    {
                        proxy.Priority = 0;
                        proxy.Status = ProxyStatus.PS_New;
                    }
                    else
                    {
                        proxy.Priority = 1;
                        proxy.Status = ProxyStatus.PS_Bad;
                    }
                    proxy.Ip = service_proxy.Ip;
                    proxy.LastUsed = defaultLastUsed;
                    proxy.IsBusy = false;
                    proxyCollection.Add(proxy);
                }
            }
            xmlWriter.Add(proxyCollection);
        }

        private HtmlProxyServerCollection GetProxiesFromService(string url)
        {
            HttpWebRequest req;
            HttpWebResponse resp;
            HtmlProxyServerCollection proxyEntities = new HtmlProxyServerCollection();
            int page = 1;
            bool lastPage = false;
            while (!lastPage)
            {
                string result = string.Empty;
                req = (HttpWebRequest)WebRequest.Create(url + "?page=" + page++);
                req.Headers.Set(HttpRequestHeader.ContentEncoding, "utf-8");

                try
                {
                    resp = (HttpWebResponse)req.GetResponse();
                    result = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                    var parser = new HtmlParser();
                    var doc = parser.Parse(result);
                    var doc_proxyArea = doc.QuerySelectorAll("table tbody tr");
                    foreach (var doc_lineNode in doc_proxyArea)
                    {
                        var doc_proxyLine = doc_lineNode.QuerySelectorAll("td");

                        //Entities formation
                        HtmlProxyServer proxyEntity = new HtmlProxyServer();
                        proxyEntity.Type = doc_proxyLine.ElementAt(5).TextContent.Split('\n')[1].Split('\r')[0];
                        if (proxyEntity.Type == "HTTPS")
                        {
                            proxyEntity.Ip = doc_proxyLine.ElementAt(1).TextContent;
                            if (proxyEntities.HtmlProxies.Where(x => x.Ip == proxyEntity.Ip).ToList().Count() == 0)
                            {
                                proxyEntity.Port = doc_proxyLine.ElementAt(2).TextContent;
                                proxyEntity.Anonymity = doc_proxyLine.ElementAt(4).TextContent;
                                NumberFormatInfo provider = new NumberFormatInfo();
                                provider.NumberDecimalSeparator = ".";
                                proxyEntity.Ping = Convert.ToDouble(doc_proxyLine.ElementAt(6).TextContent, provider);
                                proxyEntities.HtmlProxies.Add(proxyEntity);
                            }
                        }
                    }
                }
                catch
                {
                    lastPage = true;
                }
            }
            return proxyEntities;
        }
    }
}
