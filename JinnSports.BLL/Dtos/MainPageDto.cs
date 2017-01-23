using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.BLL.Dtos
{
    public class MainPageDto
    {
        public IEnumerable<NewsDto> News { get; set; }

        public IEnumerable<ResultDto> UpcomingEvents { get; set; }
    }
}
