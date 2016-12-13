using System;
using System.Collections;
using System.Collections.Generic;

namespace JinnSports.BLL.Dtos
{
    public class SportEventDto
    {
        public string SportType { get; set; }
        public DateTime Date { get; set; }
        public IDictionary<string, int> Results { get; set; } = new Dictionary<string, int>();
    }
}
