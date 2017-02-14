using System.Collections.Generic;

namespace JinnSports.BLL.Dtos
{
    public class ConformityApiViewModel
    {
        public string GroupName { get; set; }

        public List<ConformityDto> Dtos { get; set; }
    }
}
