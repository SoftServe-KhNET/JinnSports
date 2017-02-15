
using JinnSports.BLL.Dtos.SportType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.BLL.Interfaces
{
    public interface ISportTypeService
    {
        int Count(int sportTypeId, TimeSelector timeSelector);

        SportTypeSelectDto GetSportTypes(int sportTypeId, TimeSelector timeSelector, int skip, int take);

        IEnumerable<SportTypeDto> GetAllSportTypes();
    }
}
