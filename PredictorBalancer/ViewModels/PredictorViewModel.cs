using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static PredictorBalancer.Models.Predictor;

namespace PredictorBalancer.ViewModels
{
    public class PredictorViewModel
    {
        public enum Status
        {
            Available,
            Busy,
            NotAvailable
        }

        public int Id { get; set; }
        public Status CurrentStatus { get; set; }
    }
}