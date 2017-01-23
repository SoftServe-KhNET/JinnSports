using System;

namespace ScorePredictor.EventData
{
    public class TeamEvent
    {
        public int AttackScore { get; set; }
        public int DefenceScore { get; set; }
        public bool IsHomeGame { get; set; }
        public DateTime Date { get; set; }
    }
}
