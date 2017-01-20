using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.BLL.Dtos
{
    public class TeamResultsDto
    {
        public TeamDto Team { get; set; }
        public IEnumerable<ResultDto> Results { get; set; }
    }
}
