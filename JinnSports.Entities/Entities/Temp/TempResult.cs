using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.Entities.Entities.Temp
{
    public class TempResult
    {
        public int Id { get; set; }

        public Team Team { get; set; }

        public TempSportEvent TempSportEvent { get; set; }

        public ICollection<Conformity> Conformities { get; set; }

        public int? Score { get; set; }

        public bool IsHome { get; set; }
    }
}
