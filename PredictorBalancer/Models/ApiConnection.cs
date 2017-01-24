using log4net;
using Newtonsoft.Json;
using PredictorDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Xml;

namespace PredictorBalancer.Models
{
    public class ApiConnection<T> where T : class
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ApiConnection<T>));

        private string baseUrl;
        private string controllerUrn;
        private int timeoutSec;

        public ApiConnection(string baseUrl, string controllerUrn, int timeoutSec)
        {
            this.baseUrl = baseUrl;
            this.controllerUrn = controllerUrn;
            this.timeoutSec = timeoutSec;
        }

        public async void Send(T content)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Log.Info("Starting Data transfer");

                    client.BaseAddress = new Uri(this.baseUrl);
                    client.Timeout = new TimeSpan(0, 0, this.timeoutSec);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string json = JsonConvert.SerializeObject(content, Newtonsoft.Json.Formatting.Indented);
                    StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(this.controllerUrn, jsonContent);
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
                    Log.Error("Exception occured while trying to send content", ex);
                }
            }
        }
    }
}