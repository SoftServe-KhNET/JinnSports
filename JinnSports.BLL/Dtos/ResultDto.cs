using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JinnSports.BLL.Extentions;

namespace JinnSports.BLL.Dtos
{
    public class ResultDto
    {
        public ResultDto()
        {
            this.TeamNames = new List<string>(2);
            this.TeamIds = new List<int>(2);
        }
        /// <summary>
        /// Constructor with players per event
        /// </summary>
        /// <param name="eventPlayers">Max players in the event</param>
        /// 
        public ResultDto(int eventPlayers)
        {
            this.TeamNames = new List<string>(eventPlayers);
            this.TeamIds = new List<int>(eventPlayers);
        }

        public int Id { get; set; }
        public string Score { get; set; }
        public string Date { get; set; }
        public IEnumerable<string> TeamNames { get; set; }
        public IEnumerable<int> TeamIds { get; set; }
    }
}
