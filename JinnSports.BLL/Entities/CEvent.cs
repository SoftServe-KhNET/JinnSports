using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.BLL.Entities
{
    public class CEvent
    {
        public string SportType
        {
            get; set;
        }
        public DateTime Date
        {
            get; set;
        }
        public string Result
        {
            get; set;
        }
    }
}
