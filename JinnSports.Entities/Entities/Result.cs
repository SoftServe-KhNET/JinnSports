namespace JinnSports.Entities.Entities
{
    public class Result
    {
        public int Id { get; set; }
        public virtual Team Team { get; set; }
        public virtual CompetitionEvent CompetitionEvent { get; set; }
        public string Score { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Result result = (Result)obj;
            return (result.Team.Id == Team.Id) && (result.CompetitionEvent.Id == CompetitionEvent.Id);
        }

        public override string ToString()
        {
            return "Id: " + " TeamId: " + " EventId: " + this.CompetitionEvent.Id + " Score: " + this.Score;
        }
    }
}
