namespace JinnSports.Entities.Entities
{
    public class Result
    {
        public int Id { get; set; }

        public virtual Team Team { get; set; }

        public virtual SportEvent SportEvent { get; set; }

        public int Score { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}; EventId: {SportEvent.Id}; Team: {Team.Name}; Score: {Score}";
        }
    }
}
