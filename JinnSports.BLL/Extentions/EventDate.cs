using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.BLL.Extentions
{
    public class EventDate
    {
        private DateTime eventDate;

        public EventDate(DateTime dateTime)
        {
            this.eventDate = dateTime;
        }

        public override string ToString()
        {
            return this.eventDate.ToString("hh:mm:ss.FF , dd MMMM, yyyy");
        }
    }
}
