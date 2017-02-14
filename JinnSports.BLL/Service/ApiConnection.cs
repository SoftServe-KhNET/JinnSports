using log4net;
using Newtonsoft.Json;
using PredictorDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Xml;

namespace JinnSports.BLL.Service
{
    public class ApiConnection
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ApiConnection));

        private string baseUrl;
        private string controllerUrn;
        private int timeoutSec;

        public async void SendPackage(PackageDTO package)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Log.Info("Starting Data transfer");

                    this.GetConnectionSettings();

                    client.BaseAddress = new Uri(this.baseUrl);
                    client.Timeout = new TimeSpan(0, 0, this.timeoutSec);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add(WebConfigurationManager.AppSettings["ApiKey"], WebConfigurationManager.AppSettings["ApiKeyValue"]);

                    string json = JsonConvert.SerializeObject(package, Newtonsoft.Json.Formatting.Indented);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(this.controllerUrn, content);
                    if (response.IsSuccessStatusCode)
                    {
                        Log.Info("Data sucsessfully transfered");
                    }
                    else
                    {
                        Log.Info("Error occured during Data transfer");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Exception occured while trying to send SportEvents", ex);
                }
            }
        }

        private void GetConnectionSettings()
        {
            XmlDocument settings = new XmlDocument();
            settings.Load(HostingEnvironment.MapPath("~/App_Data/") + "PredictorConnection.xml");
            this.baseUrl = settings.DocumentElement.SelectSingleNode("url").InnerText;
            this.controllerUrn = settings.DocumentElement.SelectSingleNode("name").InnerText;
            this.timeoutSec = int.Parse(settings.DocumentElement.SelectSingleNode("timeout").InnerText ?? "60");
        }
    }
}
