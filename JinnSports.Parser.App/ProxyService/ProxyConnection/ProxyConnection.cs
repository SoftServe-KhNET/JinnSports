using System;
using System.Collections.Generic;
using System.Linq;
using JinnSports.Parser.App.ProxyService.ProxyEntities;
using JinnSports.Parser.App.ProxyService.ProxyRepository;
using System.Net.NetworkInformation;
using System.Net;
using System.Diagnostics;

namespace JinnSports.Parser.App.ProxyService.ProxyConnection
{
    public class ProxyConnection
    {
        public void SetStatus(string ip, bool connected)
        {
            ProxyRepository<ProxyServer> xmlWriter = new ProxyRepository<ProxyServer>();
            List<ProxyServer> proxyCollection = xmlWriter.GetAll();
            ProxyServer proxy = proxyCollection.Where(x => x.Ip == ip).FirstOrDefault();
            try
            {
                if (connected)
                {
                    if (proxy.Priority != 0)
                    {
                        proxy.Priority = 0;
                        proxy.Status = "Stable";
                    }
                    else
                    {
                        if (proxy.Status == "New")
                        {
                            proxy.Status = "Stable";
                        }
                    }
                }
                else
                {
                    switch (proxy.Priority)
                    {
                        case 0:
                            {
                                proxy.Priority++;
                                proxy.Status = "Unstable";
                                break;
                            }
                        case 1:
                            {
                                proxy.Priority++;
                                proxy.Status = "Bad";
                                break;
                            }
                        case 2:
                            {
                                proxy.Priority++;
                                proxy.Status = "Eliminated";
                                break;
                            }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            proxy.LastUsed = DateTime.Now;
            xmlWriter.Modify(proxy);
        }
        public string GetProxy()
        {
            ProxyRepository<ProxyServer> xmlWriter = new ProxyRepository<ProxyServer>();
            List<ProxyServer> proxyCollection = xmlWriter.GetAll();
            List<ProxyServer> usableProxies = proxyCollection.Where(x => x.Priority == 0 && xmlWriter.IsAvaliable(x)).ToList();
            if (usableProxies.Count == 0)
            {
                usableProxies = proxyCollection.Where(x => x.Priority == 1 && xmlWriter.IsAvaliable(x)).ToList();
                if (usableProxies.Count == 0)
                {
                    usableProxies = proxyCollection.Where(x => x.Priority == 2 && xmlWriter.IsAvaliable(x)).ToList();
                }
            }
            try
            {
                ProxyServer proxyServer = usableProxies.ElementAt(new Random().Next(0, usableProxies.Count));
                return proxyServer.Ip;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return string.Empty;
        }
        public bool CanPing(string address)
        {
            Ping ping = new Ping();

            try
            {
                PingReply reply = ping.Send(address, 2000);
                if (reply == null)
                {
                    return false;
                }

                return reply.Status == IPStatus.Success;
            }
            catch (PingException e)
            {
                return false;
            }
        }
        public HttpWebResponse MakeProxyRequest(string url, int tries)
        {
            int iter = 0;
            HttpWebResponse response;
            HttpWebRequest request;
            ProxyConnection pc = new ProxyConnection();
            while (iter < tries)
            {
                string proxy = pc.GetProxy();
                if (proxy != string.Empty)
                {
                    if (pc.CanPing(proxy) == true)
                    {
                        try
                        {
                            request = (HttpWebRequest)WebRequest.Create(url);
                            request.Headers.Set(HttpRequestHeader.ContentEncoding, "1251");
                            WebProxy webProxy = new WebProxy(proxy, true);
                            request.Proxy = webProxy;
                            response = (HttpWebResponse)request.GetResponse();
                            Debug.WriteLine("Good IP : " + proxy);
                            pc.SetStatus(proxy, true);
                            return response;
                        }
                        catch (Exception e)
                        {
                            pc.SetStatus(proxy, false);
                            iter++;
                        }
                    }
                    else
                    {
                        pc.SetStatus(proxy, false);
                        iter++;
                    }
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
    }
}

