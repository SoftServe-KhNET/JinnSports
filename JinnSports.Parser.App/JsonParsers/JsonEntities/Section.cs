using System.Collections.Generic;

namespace JinnSports.Parser.App.JsonParsers.JsonEntities
{
    public class Section : JsonObject
    {
        public int Sport { get; set; }

        public virtual IEnumerable<int> Events { get; set; }
    }
}
