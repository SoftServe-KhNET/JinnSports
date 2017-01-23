using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScorePredictor.EventData
{
    public class Prediction
    {
        public int IncomingEventId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public double HomeWinProbability { get; set; }
        public double DrawProbability { get; set; }
        public double AwayWinProbability { get; set; }
    }
}