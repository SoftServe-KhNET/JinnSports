using JinnSports.DAL.Entities;
using JinnSports.Interfaces.DALInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.DAL.Repositories
{
    class TeamsRepository : BaseRepository<Result>
    {
        public TeamsRepository()
        {

        }

        public TeamsRepository(IUnitOfWork UnitOfWork) : base(UnitOfWork)
        {
      
        }
    }
}
