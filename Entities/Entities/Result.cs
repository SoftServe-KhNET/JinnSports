using System;
using System.Collections.Generic;

namespace JinnSports.DAL.Entities
{
    public class Result
    {
        public int Id { get; set; }
        public virtual Team Team { get; set; }
        public virtual CompetitionEvent CompetitionEvent { get; set; }
        public string Score { get; set; }
    }
}
