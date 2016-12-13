using System;
using System.Collections.Generic;

namespace JinnSports.BLL.Dtos
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IDictionary<DateTime, int> Results { get; set; } = new Dictionary<DateTime, int>();
    }
}
