using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.BLL.Filters
{
    public class TeamFilter
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public ListSortDirection SortDirection { get; set; } = ListSortDirection.Ascending;

        public string SortedField { get; set; } = "TeamName";

        public string TeamName { get; set; }
    }
}
