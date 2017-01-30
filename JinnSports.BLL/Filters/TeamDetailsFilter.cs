using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.BLL.Filters
{
    public class TeamDetailsFilter
    {
        public int TeamId { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string OpponentTeamName { get; set; }

        public ListSortDirection SortDirection { get; set; }

        public string SortedField { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}
