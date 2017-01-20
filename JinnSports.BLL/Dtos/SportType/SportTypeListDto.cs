using System.Collections.Generic;

namespace JinnSports.BLL.Dtos.SportType
{
    public class SportTypeListDto
    {
        public SportTypeDto SportType { get; set; }
        public IEnumerable<ResultDto> Results { get; set; }
    }
}
