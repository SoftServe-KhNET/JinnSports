using log4net;
using Newtonsoft.Json;
using PredictorApplication.Models;
using PredictorDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Xml;

namespace PredictorApplication.Models
{
    public class ApiConnection
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ApiConnection));

        private string baseUrl;
        private string controllerUrn;
        private int timeoutSec;

        public async void SendPredictions(IEnumerable<PredictionDTO> predictions)
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
                    client.DefaultRequestHeaders.Add("X-ServiceCode", "x");

                    string json = JsonConvert.SerializeObject(predictions, Newtonsoft.Json.Formatting.Indented);
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
                    Log.Error("Exception occured while trying to send Predictions", ex);
                }
            }
        }

        private void GetConnectionSettings()
        {
            this.baseUrl = PredictorMonitor.GetInstance().CallBackURL;
            this.controllerUrn = PredictorMonitor.GetInstance().CallBackController;
            this.timeoutSec = PredictorMonitor.GetInstance().CallBackTimeout;
        }
    }
}