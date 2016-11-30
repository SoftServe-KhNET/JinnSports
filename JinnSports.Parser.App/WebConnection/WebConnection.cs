using JinnSports.Parser.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace JinnSports.Parser.App.WebConnection
{
    class WebConnection : IWebConnection
    {
        private string url;
        private string encoding = "utf-8";

        public WebConnection()
        {

        }

        public WebResponse GetResponse()
        {
            WebRequest request = WebRequest.Create(url);
            request.Headers.Set(HttpRequestHeader.ContentEncoding, encoding);
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
