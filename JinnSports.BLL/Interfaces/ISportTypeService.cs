
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
        int Count(int sportTypeId, int time);

        SportTypeSelectDto GetSportTypes(int sportTypeId, int time, int skip, int take);

        IEnumerable<SportTypeDto> GetAllSportTypes();
    }
}
