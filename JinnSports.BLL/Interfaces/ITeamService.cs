using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JinnSports.Entities;

namespace JinnSports.BLL.Interfaces
{
    public interface ITeamService
    {
        Team FindByName(string name);
    }
}
