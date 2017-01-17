using System.Net;

namespace JinnSports.Parser.App.Interfaces
{
    public interface IWebConnection
    {
        void SetURL(string url);
        void SetHeaderEncoding(string encoding);
        WebResponse GetResponse();
    }
}
