using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.Entities.Entities.Temp
{
    public class TempSportEvent
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public virtual SportType SportType { get; set; }

        public virtual ICollection<TempResult> TempResults { get; set; }
    }
}
