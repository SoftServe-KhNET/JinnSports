using System.Collections.Generic;

namespace JinnSports.Parser.App.ProxyService.ProxyInterfaces
{
    public interface IProxyRepository<T> where T : IProxyServer
    {
        void Delete(string ip);
        void Modify(T proxy);
        void Clear();
        int Count();
        void Add(T proxy);
        void Add(List<T> proxyList);
        List<T> GetAll();
        bool IsAvaliable(T proxy);
        bool Contains(string ip);
    }
}
