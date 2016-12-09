using System.Collections.Generic;

namespace JinnSports.Parser.App.JsonParsers.JsonEntities
{
    public class JsonResult
    {
        public int Id { get; set; }

        public IEnumerable<Sport> Sports { get; set; }

        public IEnumerable<Section> Sections { get; set; }

        public IEnumerable<Event> Events { get; set; }
    }
}
