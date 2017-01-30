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
        public string TeamName { get; set; }

        public ListSortDirection SortDirection { get; set; }

        public string SortedField { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}
