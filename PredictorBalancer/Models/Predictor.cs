using log4net;
using PredictorDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace PredictorBalancer.Models
{
    public class Predictor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Predictor));

        public string baseUrl;
        public string controllerUrn;
        public int timeoutSec;

        public Predictor()
        {
            
        }

        public Predictor(string baseUrl, string controllerUrn, int timeoutSec)
        {
            this.baseUrl = baseUrl;
            this.controllerUrn = controllerUrn;
            this.timeoutSec = timeoutSec;
            this.CurrentStatus = Status.NotAvailable;
        }

        public enum Status
        {
            Available,
            Busy,
            NotAvailable
        }

        public int Id { get; set; }
        public Status CurrentStatus { get; private set; }

        public void SendIncomingEvents(PackageDTO package)
        {
            ApiConnection<PackageDTO> connection = 
                new ApiConnection<PackageDTO>(this.baseUrl, this.controllerUrn, this.timeoutSec);
            Task send = new Task(() => { connection.Send(package); });
            send.Start();
            //connection.Send(package);
        }

        public async void UpdateStatus()
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

                    HttpResponseMessage response = await client.GetAsync(this.controllerUrn);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        this.CurrentStatus = Status.Available;
                        Log.Info($"Predictor #{Id} is currently available");
                    }
                    else if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                    {
                        this.CurrentStatus = Status.Busy;
                        Log.Info($"Predictor #{Id} is currently busy");
                    }
                    else
                    {
                        this.CurrentStatus = Status.NotAvailable;
                        Log.Info($"Predictor #{Id} is currently unavailable");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Exception occured while trying to update status", ex);
                }
            }
        }
    }
}