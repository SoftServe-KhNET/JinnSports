using System;

namespace JinnSports.BLL.Dtos
{
    public class CompetitionEventDto
    {
        public string SportType
        {
            get; set;
        }
        public DateTime Date
        {
            get; set;
        }
        public string Team1
        {
            get; set;
        }
        public string Result1
        {
            get; set;
        }
        public string Team2
        {
            get; set;
        }
        public string Result2
        {
            get; set;
        }
    }
}
