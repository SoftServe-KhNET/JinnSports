using System.Collections.Generic;

namespace JinnSports.BLL.Dtos.SportType
{
    public class SportTypeSelectDto
    {
        public int SelectedId { get; set; }
        public string SelectedName { get; set; }
        public IEnumerable<SportTypeDto> SportTypes { get; set; }
        public IEnumerable<SportTypeListDto> SportTypeResults { get; set; }
    }
}
