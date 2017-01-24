using System;

namespace ScorePredictor.EventData
{
    public class TeamEvent
    {
        public int AttackScore { get; set; }
        public int DefenseScore { get; set; }
        public bool IsHomeGame { get; set; }
        public DateTime Date { get; set; }
    }
}
