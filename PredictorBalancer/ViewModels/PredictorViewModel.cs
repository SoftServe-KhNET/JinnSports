using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static PredictorBalancer.Models.Predictor;

namespace PredictorBalancer.ViewModels
{
    public class PredictorViewModel
    {
        public int Id { get; set; }
        public Status CurrentStatus { get; set; }
    }
}