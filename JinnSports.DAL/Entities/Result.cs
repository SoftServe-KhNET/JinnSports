using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JinnSports.DAL.Entities
{
    public class Result
    {
        public int Id { get; set; }
        [Required]
        public virtual Team Team { get; set; }
        [Required]
        public virtual CompetitionEvent CompetitionEvent { get; set; }
        public string Score { get; set; }
    }
}
