using JinnSports.Parser.App.ProxyService.ProxyInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using JinnSports.Parser.App.ProxyService.ProxyConnections;
using System.Threading;
using JinnSports.Parser.App.Configuration.Proxy;
using JinnSports.Parser.App.WebConnection;

namespace JinnSports.Parser.App.ProxyService.ProxyTerminal
{
    public class ProxyAsync : IProxyAsync
    {
        private Uri url;
        private IProxyConnection pc;
        private CancellationTokenSource cancelTokenSrc;
        private int asyncinterval;
        private int timeout;

        public ProxyAsync(IProxyConnection proxyConnection, Uri url)
        {
            this.asyncinterval = ProxySettings.GetAsyncInterval();
            this.timeout = ProxySettings.GetTimeout();
            this.pc = proxyConnection;
            this.cancelTokenSrc = new CancellationTokenSource();
            this.url = url;
        }

        public ProxyHttpWebResponse GetProxyAsync()
        {
            return this.GetProxyAsync(false);
        }

        public ProxyHttpWebResponse GetProxyAsync(bool asyncResponse)
        {
            IList<Task<ProxyHttpWebResponse>> tasks = new List<Task<ProxyHttpWebResponse>>();

            //Creating tasks while CancelationToken is not cancelled
            while (!this.cancelTokenSrc.Token.IsCancellationRequested)
            {
                Task.Delay(this.asyncinterval * 1000).Wait();

                tasks.Add(Task<ProxyHttpWebResponse>.Factory.StartNew(() =>
                {
                    var result = this.pc.GetProxyResponse(url, this.timeout, cancelTokenSrc.Token, asyncResponse);
                    if (result != null)
                    {
                        cancelTokenSrc.Cancel();
                    }
                    return result;
                }
                , this.cancelTokenSrc.Token));
            }

            //Waiting for finishing all running tasks except Canceled
            tasks = tasks.Where(t => t.Status != TaskStatus.Canceled).ToList();

            Task.WaitAll(tasks.ToArray());

            //Checking for Status = RanToCompletion
            foreach (Task<ProxyHttpWebResponse> task in tasks)
            {
                if (task.Result != null)
                {
                    //returning valid IP, if found, can be only one for this function
                    return task.Result as ProxyHttpWebResponse;
                }
            }
            return null;
        }
    }
}
