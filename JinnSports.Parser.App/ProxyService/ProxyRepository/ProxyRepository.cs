﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using JinnSports.Parser.App.ProxyService.ProxyInterfaces;
using JinnSports.Parser.App.ProxyService.ProxyEnities;

namespace JinnSports.Parser.App.ProxyService.ProxyRepository
{
    public class ProxyRepository<T> : IProxyRepository<T> where T : IProxyServer
    {
        private XmlSerializer xmlSerializer;
        private string path;

        public ProxyRepository(string path)
        {
            this.path = path;
            xmlSerializer = new XmlSerializer(typeof(List<ProxyServer>));
        }
        public void Delete(string ip)
        {
            List<T> proxyCollection = new List<T>();
            TextReader xmlReader = new StreamReader(path);
            try
            {
                proxyCollection = (List<T>)xmlSerializer.Deserialize(xmlReader);
            }
            catch
            {
            }
            finally
            {
                xmlReader.Close();
            }
            proxyCollection.Remove(proxyCollection.FirstOrDefault(x => x.Ip == ip));
            TextWriter xmlWriter = new StreamWriter(path);
            xmlSerializer.Serialize(xmlWriter, proxyCollection);
            xmlWriter.Close();
        }
        public void Modify(T proxy)
        {
            List<T> proxyCollection = new List<T>();
            TextReader xmlReader = new StreamReader(path);
            try
            {
                proxyCollection = (List<T>)xmlSerializer.Deserialize(xmlReader);
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
            TextWriter xmlWriter = new StreamWriter(path);
            xmlSerializer.Serialize(xmlWriter, proxyCollection);
            xmlWriter.Close();
        }
        public void Clear()
        {
            List<T> proxyCollection = new List<T>();
            TextWriter xmlWriter = new StreamWriter(path);
            xmlSerializer.Serialize(xmlWriter, proxyCollection);
            xmlWriter.Close();
        }
        public int Count()
        {
            List<T> proxyCollection = new List<T>();
            TextReader xmlReader = new StreamReader(path);
            try
            {
                proxyCollection = (List<T>)xmlSerializer.Deserialize(xmlReader);
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
            TextReader xmlReader = new StreamReader(path);
            try
            {
                proxyCollection = (List<T>)xmlSerializer.Deserialize(xmlReader);
            }
            catch
            {
            }
            finally
            {
                xmlReader.Close();
            }
            proxyCollection.Add(proxy);
            TextWriter xmlWriter = new StreamWriter(path);
            xmlSerializer.Serialize(xmlWriter, proxyCollection);
            xmlWriter.Close();
        }
        public List<T> GetAll()
        {
            List<T> proxyCollection = new List<T>();
            TextReader xmlReader = new StreamReader(path);
            try
            {
                proxyCollection = (List<T>)xmlSerializer.Deserialize(xmlReader);
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
        public bool isAvaliable(T proxy)
        {
            TimeSpan timeDifference = DateTime.Now.TimeOfDay - proxy.LastUsed.TimeOfDay;
            if (Math.Abs(timeDifference.Seconds + timeDifference.Minutes * 60 + timeDifference.Hours * 3600 + timeDifference.Days * 3600 * 24) >= 20 * 60)
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
            TextReader xmlReader = new StreamReader(path);
            try
            {
                proxyCollection = (List<T>)xmlSerializer.Deserialize(xmlReader);
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