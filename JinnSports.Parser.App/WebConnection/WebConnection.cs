using System.Net;
using JinnSports.Parser.App.Interfaces;

namespace JinnSports.Parser.App.WebConnection
{
    public class WebConnection : IWebConnection
    {
        private string url;
        private string encoding = "utf-8";

        public WebResponse GetResponse()
        {
            WebRequest request = WebRequest.Create(this.url);
            request.Headers.Set(HttpRequestHeader.ContentEncoding, this.encoding);
            return request.GetResponse();
        }

        public void SetHeaderEncoding(string encoding)
        {
            this.encoding = encoding;
        }

        public void SetURL(string url)
        {
            this.url = url;
        }
    }
}
