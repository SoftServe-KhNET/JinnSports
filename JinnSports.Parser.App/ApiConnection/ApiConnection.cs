﻿using DTO.JSON;
using JinnSports.Parser.App.Exceptions;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Xml;

namespace JinnSports.Parser.App
{
    public class ApiConnection
    {
        private static readonly ILog Log = LogManager.GetLogger("AppLog");

        private string baseUrl;
        private string controllerUrn;
        private int timeoutSec;

        /// <summary>
        /// Accepts collection of SportEventDTO and try to serialize and send it to Api Controller
        /// 
        /// </summary>
        /// <param name="events"></param>
        /// <exception cref="SaveDataException"></exception>
        public void SendEvents(ICollection<SportEventDTO> events)
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

                    string json = JsonConvert.SerializeObject(events, Newtonsoft.Json.Formatting.Indented);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    Task<HttpResponseMessage> postResponseTask = client.PostAsync(this.controllerUrn, content);

                    postResponseTask.ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            Log.Info("Error occured during Data transfer");
                        }
                    });

                    HttpResponseMessage response = postResponseTask.Result as HttpResponseMessage;

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
            settings.Load(HostingEnvironment.MapPath("~/App_Data/") + "ApiConnection.xml");
            this.baseUrl = settings.DocumentElement.SelectSingleNode("url").InnerText;
            this.controllerUrn = settings.DocumentElement.SelectSingleNode("name").InnerText;
            this.timeoutSec = int.Parse(settings.DocumentElement.SelectSingleNode("timeout").InnerText ?? "60");
        }
    }
}