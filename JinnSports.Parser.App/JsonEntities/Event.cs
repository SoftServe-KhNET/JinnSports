using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.Parser.App.JsonEntities
{
    public class Event : JsonObject
    {
        public long StartTime { get; set; }

        public string Score { get; set; }

        public int Status { get; set; }

        public string Comment1 { get; set; }

        public string Comment2 { get; set; }

        public string Comment3 { get; set; }

        public string GoalOrder { get; set; }
    }
}
