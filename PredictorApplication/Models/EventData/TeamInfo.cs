using System.Collections.Generic;

namespace ScorePredictor.EventData
{
    public class TeamInfo
    {
        public int TeamId { get; set; }
        public bool IsHomeGame { get; set; }
        public IEnumerable<TeamEvent> TeamEvents { get; set; }
    }
}
