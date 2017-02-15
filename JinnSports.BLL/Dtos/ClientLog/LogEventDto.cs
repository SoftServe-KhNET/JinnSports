using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.BLL.Dtos.ClientLog
{
    public class LogEventDto
    {
        public long Time { get; set; }

        public string Event { get; set; }

        public string TagName { get; set; }

        public string Id { get; set; }

        public string Value { get; set; }

        public int CoordX { get; set; }

        public int CoordY { get; set; }
    }
}
