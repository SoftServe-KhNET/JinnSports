using System.Collections.Generic;

namespace JinnSports.WEB.Models
{
    public class TeamDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ResultDetailsViewModel> Results { get; set; }
    }
}