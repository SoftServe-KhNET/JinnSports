using System.Collections.Generic;

namespace JinnSports.BLL.Dtos
{
    public class TeamDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ResultDto> Results { get; set; }
    }
}
