using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using JinnSports.Parser.App.ProxyService.ProxyEntities;
using JinnSports.Parser.App.ProxyService.ProxyInterfaces;
using JinnSports.Parser.App.Configuration.Proxy;
using System.Web.Hosting;

namespace JinnSports.Parser.App.ProxyService.ProxyRepository
{
    public class ProxyRepository<T> : IProxyRepository<T> where T : IProxyServer
    {
        private static object repLocker = new object();
        private XmlSerializer xmlSerializer;
        private string path;

        public ProxyRepository()
        {
            this.path = HostingEnvironment.MapPath("~/App_Data/") + ProxySettings.GetPath();
            this.xmlSerializer = new XmlSerializer(typeof(List<ProxyServer>));
            this.Interval = ProxySettings.GetCooldown();
        }

        public int Interval { get; private set; }

        public void Delete(string ip)
        {
            List<T> proxyCollection = new List<T>();
            lock (repLocker)
            {
                using (TextReader xmlReader = new StreamReader(this.path))
                {
                    proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
                }

                proxyCollection.Remove(proxyCollection.FirstOrDefault(x => x.Ip == ip));

                using (TextWriter xmlWriter = new StreamWriter(this.path))
                {
                    this.xmlSerializer.Serialize(xmlWriter, proxyCollection);
                }
            }
        }

        public void Modify(T proxy)
        {
            List<T> proxyCollection = new List<T>();
            lock (repLocker)
            {
                using (TextReader xmlReader = new StreamReader(this.path))
                {
                    proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
                }
                int index = proxyCollection.FindIndex(x => x.Ip == proxy.Ip);
                proxyCollection.RemoveAt(index);
                proxyCollection.Insert(index, proxy);
                using (TextWriter xmlWriter = new StreamWriter(this.path))
                {
                    this.xmlSerializer.Serialize(xmlWriter, proxyCollection);
                }
            }
        }

        public void Modify(List<T> proxyList)
        {
            List<T> proxyCollection = new List<T>();
            lock (repLocker)
            {
                using (TextReader xmlReader = new StreamReader(this.path))
                {
                    proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
                }
                foreach (T proxy in proxyList)
                {
                    int index = proxyCollection.FindIndex(x => x.Ip == proxy.Ip);
                    proxyCollection.RemoveAt(index);
                    proxyCollection.Insert(index, proxy);
                }
                using (TextWriter xmlWriter = new StreamWriter(this.path))
                {
                    this.xmlSerializer.Serialize(xmlWriter, proxyCollection);
                }
            }
        }

        public void Clear()
        {
            List<T> proxyCollection = new List<T>();
            lock (repLocker)
            {
                using (TextWriter xmlWriter = new StreamWriter(this.path))
                {
                    this.xmlSerializer.Serialize(xmlWriter, proxyCollection);
                }
            }
        }

        public int Count()
        {
            List<T> proxyCollection = new List<T>();
            lock (repLocker)
            {
                using (TextReader xmlReader = new StreamReader(this.path))
                {
                    proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
                }
            }
            int a = proxyCollection.Count();
            return proxyCollection.Count();
        }

        public void Add(T proxy)
        {
            List<T> proxyCollection = new List<T>();
            lock (repLocker)
            {
                using (TextReader xmlReader = new StreamReader(this.path))
                {
                    proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
                }
                proxyCollection.Add(proxy);
                using (TextWriter xmlWriter = new StreamWriter(this.path))
                {
                    this.xmlSerializer.Serialize(xmlWriter, proxyCollection);
                }
            }
        }

        public void Add(List<T> proxyList)
        {
            List<T> proxyCollection = new List<T>();
            lock (repLocker)
            {
                using (TextReader xmlReader = new StreamReader(this.path))
                {
                    proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
                }
                foreach (T proxy in proxyList)
                {
                    proxyCollection.Add(proxy);
                }
                using (TextWriter xmlWriter = new StreamWriter(this.path))
                {
                    this.xmlSerializer.Serialize(xmlWriter, proxyCollection);
                }
            }
        }

        public List<T> GetAll()
        {
            List<T> proxyCollection = new List<T>();
            lock (repLocker)
            {
                using (TextReader xmlReader = new StreamReader(this.path))
                {
                    proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
                }
            }
            return proxyCollection;
        }

        public bool IsAvaliable(T proxy)
        {
            TimeSpan timeDifference = DateTime.Now.TimeOfDay - proxy.LastUsed.TimeOfDay;
            double proxyTimeout = timeDifference.TotalSeconds;
            if ((Math.Abs(proxyTimeout) >= this.Interval) && (proxy.IsBusy != true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Contains(string ip)
        {
            List<T> proxyCollection = new List<T>();

            lock (repLocker)
            {
                using (TextReader xmlReader = new StreamReader(this.path))
                {
                    proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
                }
                if (proxyCollection.Where(x => x.Ip == ip).Count() != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}