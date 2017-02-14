using JinnSports.BLL.Dtos.Validators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JinnSports.BLL.Dtos
{    
    public class ConformityViewModel
    {
        public ConformityViewModel(string inputName, int inputNameId, List<ConformityDto> conformities)
        {
            this.InputName = inputName;
            this.Conformities = conformities;
            this.InputNameId = inputNameId;
        }

        public ConformityViewModel()
        {
        }
        
        public int? ConformityId { get; set; }

        public int InputNameId { get; set; }

        public string InputName { get; set; }
        
        [Display(Name = "Matching variant")]
        public string ExistedName { get; set; }

        public List<ConformityDto> Conformities { get; set; }
    }
}
