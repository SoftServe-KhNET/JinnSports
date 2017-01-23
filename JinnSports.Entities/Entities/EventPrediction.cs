using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.Entities.Entities
{
    public class EventPrediction
    {
        public int Id { get; set; }
        
        public virtual Team HomeTeam { get; set; }

        public virtual Team AwayTeam { get; set; }

        public virtual SportEvent SportEvent { get; set; }

        public double HomeWinProbability { get; set; }

        public double DrawProbability { get; set; }

        public double AwayWinProbability { get; set; }
    }
}
