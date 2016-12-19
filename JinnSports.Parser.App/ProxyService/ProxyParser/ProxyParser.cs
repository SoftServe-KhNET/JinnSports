using System;
using System.Linq;
using JinnSports.Parser.App.ProxyService.ProxyRepository;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Globalization;
using System.Collections.Generic;
using JinnSports.Parser.App.ProxyService.ProxyEntities;

namespace JinnSports.Parser.App.ProxyService.ProxyParser
{
    public class ProxyParser
    {
        public void UpdateData(bool clearData, string url)
        {
            ProxyRepository<ProxyServer> writer = new ProxyRepository<ProxyServer>();
            if (clearData)
            {
                writer.Clear();
            }
            HtmlProxyServerCollection service_proxies = this.GetProxiesFromService(url);
            this.SaveProxiesToXml(service_proxies);
        }
        public void UpdateData(string url)
        {
            HtmlProxyServerCollection service_proxies = this.GetProxiesFromService(url);
            this.SaveProxiesToXml(service_proxies);
        }
        private void SaveProxiesToXml(HtmlProxyServerCollection service_proxies)
        {
            ProxyRepository<ProxyServer> xmlWriter = new ProxyRepository<ProxyServer>();
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
                        proxy.Status = "New";
                    }
                    else
                    {
                        proxy.Priority = 1;
                        proxy.Status = "Bad";
                    }
                    proxy.Ip = service_proxy.Ip;
                    proxy.LastUsed = defaultLastUsed;
                    //здесь приоритет будет 1 также если низкая защищенность, после разбора кодировки
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
                //проход по всему сайту
                req = (HttpWebRequest)WebRequest.Create(url + "?page=" + page++);

                //test block
                /*{
                    req = (HttpWebRequest)WebRequest.Create(url + "?page=5");
                    lastPage = true;
                }*/

                req.Headers.Set(HttpRequestHeader.ContentEncoding, "utf-8");
                resp = (HttpWebResponse)req.GetResponse();
                result = new StreamReader(resp.GetResponseStream()).ReadToEnd();
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(result);
                HtmlNode doc_proxyArea = doc.DocumentNode.SelectSingleNode("//table/tbody");
                try
                {
                    //int a = 0;
                    foreach (HtmlNode doc_lineNode in doc_proxyArea.SelectNodes("tr"))
                    {
                        //test
                        {
                            /*if (a == 8)
                            {
                                lastPage = true;
                                break;
                            }
                            a++;*/
                        }
                        HtmlNodeCollection doc_proxyLine = doc_lineNode.SelectNodes("td");

                        //Entity's object formation
                        HtmlProxyServer proxyEntity = new HtmlProxyServer();
                        proxyEntity.Type = doc_proxyLine.ElementAt(5).InnerText.Split('\n')[1].Split('\r')[0];
                        if (proxyEntity.Type == "HTTPS")
                        {
                            proxyEntity.Ip = doc_proxyLine.ElementAt(1).InnerText;
                            proxyEntity.Port = doc_proxyLine.ElementAt(2).InnerText;
                            proxyEntity.Anonymity = doc_proxyLine.ElementAt(4).InnerText;
                            NumberFormatInfo provider = new NumberFormatInfo();
                            provider.NumberDecimalSeparator = ".";
                            proxyEntity.Ping = Convert.ToDouble(doc_proxyLine.ElementAt(6).InnerText, provider);
                            proxyEntities.HtmlProxies.Add(proxyEntity);
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
