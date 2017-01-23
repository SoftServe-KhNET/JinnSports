using System.Collections.Generic;

namespace ScorePredictor.EventData
{
    public class IncomingEvent
    {
        public int Id { get; set; }
        public string SportType { get; set; }
        public IEnumerable<TeamInfo> TeamsInfo { get; set; }
    }
}
