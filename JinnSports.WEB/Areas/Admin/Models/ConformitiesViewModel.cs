using System.Collections.Generic;

namespace JinnSports.WEB.Areas.Admin.Models
{
    public class ConformitiesViewModel
    {
        public ConformitiesViewModel(string inputName, List<string> existedNames)
        {
            this.InputName = inputName;
            this.ExistedNames = existedNames;
        }

        public string InputName { get; set; }
        public string ExistedName { get; set; }
        public List<string> ExistedNames { get; set; }
    }
}