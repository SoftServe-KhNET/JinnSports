using System;
using System.Collections;
using System.Collections.Generic;

namespace JinnSports.BLL.Dtos
{
    public class SportEventDto
    {
        public int TeamFirstId { get; set; }

        public string TeamFirst { get; set; }

        public string ScoreFirst { get; set; }

        public int TeamSecondId { get; set; }

        public string TeamSecond { get; set; }

        public string ScoreSecond { get; set; }

        public string Date { get; set; }        
    }
}
