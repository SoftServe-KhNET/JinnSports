using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using JinnSports.Parser.App.ProxyService.ProxyEntities;
using JinnSports.Parser.App.ProxyService.ProxyInterfaces;

namespace JinnSports.Parser.App.ProxyService.ProxyRepository
{
    public class ProxyRepository<T> : IProxyRepository<T> where T : IProxyServer
    {
        private XmlSerializer xmlSerializer;

        private string path;

        public ProxyRepository()
        {
            this.path = @"..\..\" + ConfigSettings.Xml();
            this.xmlSerializer = new XmlSerializer(typeof(List<ProxyServer>));
            this.Interval = 1;
        }

        public int Interval { get; private set; }

        public void Delete(string ip)
        {
            List<T> proxyCollection = new List<T>();
            TextReader xmlReader = new StreamReader(this.path);
            try
            {
                proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
            }
            catch
            {
            }
            finally
            {
                xmlReader.Close();
            }
            proxyCollection.Remove(proxyCollection.FirstOrDefault(x => x.Ip == ip));
            TextWriter xmlWriter = new StreamWriter(this.path);
            this.xmlSerializer.Serialize(xmlWriter, proxyCollection);
            xmlWriter.Close();
        }
        public void Modify(T proxy)
        {
            List<T> proxyCollection = new List<T>();
            TextReader xmlReader = new StreamReader(this.path);
            try
            {
                proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
            }
            catch
            {
            }
            finally
            {
                xmlReader.Close();
            }
            int index = proxyCollection.FindIndex(x => x.Ip == proxy.Ip);
            proxyCollection.RemoveAt(index);
            proxyCollection.Insert(index, proxy);
            TextWriter xmlWriter = new StreamWriter(this.path);
            this.xmlSerializer.Serialize(xmlWriter, proxyCollection);
            xmlWriter.Close();
        }
        public void Clear()
        {
            List<T> proxyCollection = new List<T>();
            TextWriter xmlWriter = new StreamWriter(this.path);
            this.xmlSerializer.Serialize(xmlWriter, proxyCollection);
            xmlWriter.Close();
        }
        public int Count()
        {
            List<T> proxyCollection = new List<T>();
            TextReader xmlReader = new StreamReader(this.path);
            try
            {
                proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
            }
            catch
            {
            }
            finally
            {
                xmlReader.Close();
            }
            int a = proxyCollection.Count();
            return proxyCollection.Count();
        }
        public void Add(T proxy)
        {
            List<T> proxyCollection = new List<T>();
            TextReader xmlReader = new StreamReader(this.path);
            try
            {
                proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
            }
            catch
            {
            }
            finally
            {
                xmlReader.Close();
            }
            proxyCollection.Add(proxy);
            TextWriter xmlWriter = new StreamWriter(this.path);
            this.xmlSerializer.Serialize(xmlWriter, proxyCollection);
            xmlWriter.Close();
        }
        public void Add(List<T> proxyList)
        {
            List<T> proxyCollection = new List<T>();
            TextReader xmlReader = new StreamReader(this.path);
            try
            {
                proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
            }
            catch
            {
            }
            finally
            {
                xmlReader.Close();
            }
            foreach (T proxy in proxyList)
            {
                proxyCollection.Add(proxy);
            }
            TextWriter xmlWriter = new StreamWriter(this.path);
            this.xmlSerializer.Serialize(xmlWriter, proxyCollection);
            xmlWriter.Close();
        }
        public List<T> GetAll()
        {
            List<T> proxyCollection = new List<T>();
            TextReader xmlReader = new StreamReader(this.path);
            try
            {
                proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
            }
            catch
            {
            }
            finally
            {
                xmlReader.Close();
            }
            return proxyCollection;
        }
        public bool IsAvaliable(T proxy)
        {
            TimeSpan timeDifference = DateTime.Now.TimeOfDay - proxy.LastUsed.TimeOfDay;
            var proxyTimeout = timeDifference.Seconds + (timeDifference.Minutes * 60) + (timeDifference.Hours * 3600) + (timeDifference.Days * 3600 * 24);
            if (Math.Abs(proxyTimeout) >= this.Interval * 60)
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
            TextReader xmlReader = new StreamReader(this.path);
            try
            {
                proxyCollection = (List<T>)this.xmlSerializer.Deserialize(xmlReader);
            }
            catch
            {
            }
            finally
            {
                xmlReader.Close();
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
