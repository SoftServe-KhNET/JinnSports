using System.Collections.Generic;

namespace JinnSports.WEB.Models
{
    public class SportResultsViewModel
    {
        public string SportName { get; set; }

        public IList<ResultViewModel> Events { get; set; }       
    }
}