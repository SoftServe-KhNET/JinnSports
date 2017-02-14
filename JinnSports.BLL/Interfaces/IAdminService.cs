using JinnSports.BLL.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace JinnSports.BLL.Interfaces
{
    public interface IAdminService
    {
        List<ConformityDto> GetConformities();
        AdminApiViewModel GetConformityApiViewModel();        
        ConformityViewModel GetConformityViewModel(int id);
        void Save(ConformityViewModel model);
        void Save(string inputName, string existedName);
    }
}
