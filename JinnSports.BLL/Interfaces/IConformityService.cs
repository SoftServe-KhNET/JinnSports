using JinnSports.BLL.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace JinnSports.BLL.Interfaces
{
    public interface IConformityService
    {
        List<string> GetConformities();
        ConformityViewModel GetConformityViewModel(string inputName);
        void Save(ConformityViewModel model);
    }
}
