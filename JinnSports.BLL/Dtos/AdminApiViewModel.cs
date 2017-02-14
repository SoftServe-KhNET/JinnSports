using System.Collections.Generic;

namespace JinnSports.BLL.Dtos
{
    public class AdminApiViewModel
    {
        public List<ConformityApiViewModel> Conformities { get; set; }
        public List<string> Names { get; set; }
    }
}
