using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.BLL.Dtos
{
    public class EventPredictionDto
    {
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string EventDate { get; set; }
        public string EventType { get; set; }
        public string HomeWinProbability { get; set; }
        public string AwayWinProbability { get; set; }
        public string DrawProbability { get; set; }
    }
}
