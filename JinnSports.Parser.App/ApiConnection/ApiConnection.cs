using DTO.JSON;
using JinnSports.Parser.App.Exceptions;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;

namespace JinnSports.Parser
{
    public class ApiConnection
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ApiConnection));

        private string baseUrl;
        private string controllerUrn;
        private int timeoutSec;

        /// <summary>
        /// Accepts collection of SportEventDTO and try to serialize and send it to Api Controller
        /// 
        /// </summary>
        /// <param name="events"></param>
        /// <exception cref="SaveDataException"></exception>
        public async void SendEvents(ICollection<SportEventDTO> events)
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

                    string json = JsonConvert.SerializeObject(events, Newtonsoft.Json.Formatting.Indented);
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
                    throw new SaveDataException("Exception occured while trying to send SportEvents", ex);
                }
            }
        }

        private void GetConnectionSettings()
        {
            XmlDocument settings = new XmlDocument();
            settings.Load("ApiConnection.xml");
            this.baseUrl = settings.DocumentElement.SelectSingleNode("url").InnerText;
            this.controllerUrn = settings.DocumentElement.SelectSingleNode("name").InnerText;
            this.timeoutSec = int.Parse(settings.DocumentElement.SelectSingleNode("timeout").InnerText ?? "60");
        }
    }
}