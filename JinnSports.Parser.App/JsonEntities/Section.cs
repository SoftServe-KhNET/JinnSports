using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.Parser.App.JsonEntities
{
    public class Section : JsonObject
    {
        public int Sport { get; set; }

        public virtual IEnumerable<int> Events { get; set; }
    }
}
