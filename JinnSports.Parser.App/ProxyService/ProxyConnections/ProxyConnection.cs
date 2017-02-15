﻿using System;
using System.Collections.Generic;
using System.Linq;
using JinnSports.Parser.App.ProxyService.ProxyEntities;
using JinnSports.Parser.App.ProxyService.ProxyRepository;
using System.Net.NetworkInformation;
using System.Net;
using System.Diagnostics;
using JinnSports.Parser.App.ProxyService.ProxyInterfaces;
using System.Threading;
using JinnSports.Parser.App.ProxyService.ProxyEnums;
using System.Threading.Tasks;
using JinnSports.Parser.App.WebConnection;
using log4net;

namespace JinnSports.Parser.App.ProxyService.ProxyConnections
{
    public class ProxyConnection : IProxyConnection
    {
        private static readonly ILog Log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static object connectionLocker = new object();
        private ProxyRepository<ProxyServer> xmlWriter;

        public ProxyConnection()
        {
            this.xmlWriter = new ProxyRepository<ProxyServer>();
        }

        public void UpdateElimination()
        {
            lock (connectionLocker)
            {
                try
                {
                    ProxyRepository<ProxyServer> xmlWriter = new ProxyRepository<ProxyServer>();
                    List<ProxyServer> eliminatedCollection = xmlWriter.GetAll().Where(x => x.Status == ProxyStatus.PS_Eliminated).ToList();
                    foreach (ProxyServer proxy in eliminatedCollection)
                    {
                        proxy.Status = ProxyStatus.PS_Bad;
                        proxy.Priority--;
                    }
                    xmlWriter.Modify(eliminatedCollection);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
        }

        public void SetStatus(string ip, ConnectionStatus status)
        {
            lock (connectionLocker)
            {
                List<ProxyServer> proxyCollection = this.xmlWriter.GetAll();
                ProxyServer proxy = proxyCollection.Where(x => x.Ip == ip).FirstOrDefault();
                try
                {
                    switch (status)
                    {
                        case ConnectionStatus.CS_Connected:
                            {
                                if (proxy.Priority != 0)
                                {
                                    proxy.Priority = 0;
                                    proxy.Status = ProxyStatus.PS_Stable;
                                }
                                else
                                {
                                    if (proxy.Status == ProxyStatus.PS_New)
                                    {
                                        proxy.Status = ProxyStatus.PS_Stable;
                                    }
                                }
                                proxy.LastUsed = DateTime.Now;
                                proxy.IsBusy = false;
                                break;
                            }
                        case ConnectionStatus.CS_Disconnected:
                            {
                                switch (proxy.Priority)
                                {
                                    case 0:
                                        {
                                            proxy.Status = ProxyStatus.PS_Unstable;
                                            proxy.Priority++;
                                            break;
                                        }
                                    case 1:
                                        {
                                            proxy.Status = ProxyStatus.PS_Bad;
                                            proxy.Priority++;
                                            break;
                                        }
                                    case 2:
                                        {
                                            proxy.Status = ProxyStatus.PS_Eliminated;
                                            proxy.Priority++;
                                            break;
                                        }
                                }
                                proxy.LastUsed = DateTime.Now;
                                proxy.IsBusy = false;
                                break;
                           }
                        case ConnectionStatus.CS_PreResponseTerminated:
                            {
                                proxy.IsBusy = false;
                                break;
                            }
                        case ConnectionStatus.CS_PostResponseTerminated:
                            {
                                if (proxy.Priority != 0)
                                {
                                    proxy.Priority = 0;
                                    proxy.Status = ProxyStatus.PS_Stable;
                                }
                                else
                                {
                                    if (proxy.Status == ProxyStatus.PS_New)
                                    {
                                        proxy.Status = ProxyStatus.PS_Stable;
                                    }
                                }
                                proxy.LastUsed = DateTime.Now;
                                proxy.IsBusy = false;
                                break;
                            }
                        case ConnectionStatus.CS_СonnectedWrongly:
                            {
                                proxy.IsBusy = false;
                                proxy.Priority = 4;
                                proxy.LastUsed = DateTime.Now;
                                proxy.Status = ProxyStatus.PS_Wrong;
                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
                this.xmlWriter.Modify(proxy);
            }
        }

        public string GetProxy()
        {
            lock (connectionLocker)
            {
                List<ProxyServer> proxyCollection = this.xmlWriter.GetAll();
                List<ProxyServer> usableProxies = proxyCollection.Where(x => x.Priority == 0 && this.xmlWriter.IsAvaliable(x)).ToList();
                if (usableProxies.Count == 0)
                {
                    usableProxies = proxyCollection.Where(x => x.Priority == 1 && this.xmlWriter.IsAvaliable(x)).ToList();
                    if (usableProxies.Count == 0)
                    {
                        usableProxies = proxyCollection.Where(x => x.Priority == 2 && this.xmlWriter.IsAvaliable(x)).ToList();
                    }
                }
                try
                {
                    ProxyServer proxyServer = usableProxies.ElementAt(new Random().Next(0, usableProxies.Count));
                    proxyServer.IsBusy = true;
                    this.xmlWriter.Modify(proxyServer);
                    return proxyServer.Ip;
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
                return string.Empty;
            }
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
            catch (PingException)
            {
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ProxyHttpWebResponse GetProxyResponse(Uri url, int timeout, CancellationToken token, bool async)
        {
            HttpWebResponse response;
            HttpWebRequest request;
            ProxyHttpWebResponse proxyResponse = new ProxyHttpWebResponse();
            // Get proxy from xml file, string "proxy" contains the result
            string proxy = this.GetProxy();

            if (proxy == string.Empty)
            {
                this.UpdateElimination();
                proxy = this.GetProxy();
            }

            if (proxy != string.Empty)
            {
                //pinging block, if server doesn't ping SetStatus (Disconnected)
                if (this.CanPing(proxy))
                {
                    try
                    {
                        request = (HttpWebRequest)WebRequest.Create(url);
                        request.Headers.Set(HttpRequestHeader.ContentEncoding, "1251");
                        WebProxy webProxy = new WebProxy(proxy, false);
                        webProxy.Credentials = new NetworkCredential("user", "pass", "dom");
                        request.Proxy = webProxy;

                        //PreRequest CancellationToken checking, if Cancel - SetStatus (PreResponseTerminated)
                        if (token.IsCancellationRequested)
                        {
                            this.SetStatus(proxy, ConnectionStatus.CS_PreResponseTerminated);
                            return null;
                        }

                        if (async)
                        {
                            response = this.RunAsyncProxyConnection(request, timeout, proxy);
                        }
                        else
                        {
                            response = this.RunProxyConnection(request, timeout, proxy);
                        }

                        //PostRequest CancellationToken checking, if Cancel - SetStatus (PostResponseTerminated)
                        if (token.IsCancellationRequested)
                        {
                            this.SetStatus(proxy, ConnectionStatus.CS_PostResponseTerminated);
                            return null;
                        }
                        if (response != null)
                        {
                            //Valid Ip founded, SetStatus (Connected)
                            this.SetStatus(proxy, ConnectionStatus.CS_Connected);
                            proxyResponse.Proxy = proxy;
                            proxyResponse.Response = response;
                            return proxyResponse;
                        }
                        else
                        {
                            //Invalid Ip founded, SetStatus (Disconnected). Unreachable code
                            this.SetStatus(proxy, ConnectionStatus.CS_Disconnected);
                        }
                    }
                    catch (Exception e)
                    {
                        //Connection Exception, SetStatus (Disconnected)
                        this.SetStatus(proxy, ConnectionStatus.CS_Disconnected);
                    }
                }
                else
                {
                    //Connection Exception, SetStatus (Disconnected)
                    this.SetStatus(proxy, ConnectionStatus.CS_Disconnected);
                }
            }
            return null;
        }
  
        private HttpWebResponse RunAsyncProxyConnection(HttpWebRequest request, int timeout, string proxy)
        {
            Task<WebResponse> task = Task.Factory.FromAsync(
                       request.BeginGetResponse,
                       request.EndGetResponse,
                       request);

            if (!task.Wait(timeout * 1000))
            {
                throw new WebException(
                    "No response was received during the time-out period specified.",
                    WebExceptionStatus.Timeout);
            }

            task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    proxy = ((t.AsyncState as HttpWebRequest).Proxy as WebProxy).Address.Host;
                    this.SetStatus(proxy, ConnectionStatus.CS_Disconnected);
                }
            });

            return task.Result as HttpWebResponse;
        }

        private HttpWebResponse RunProxyConnection(HttpWebRequest request, int timeout, string proxy)
        {
            request.Timeout = timeout * 1000;
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            return response;
        }
    }
}