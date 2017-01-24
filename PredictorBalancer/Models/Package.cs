using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PredictorBalancer.Models
{
    public class Package
    {
        public string CallBackURL { get; set; }
        public string CallBackController { get; set; }
        public int CallBackTimeout { get; set; }
    }
}