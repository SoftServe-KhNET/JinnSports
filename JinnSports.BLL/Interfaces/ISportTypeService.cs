
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
        int Count(int sportTypeId);

        SportTypeSelectDto GetSportTypes(int sportTypeId, int time);

        IEnumerable<SportTypeDto> GetAllSportTypes();
    }
}
