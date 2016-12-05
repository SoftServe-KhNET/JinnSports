using JinnSports.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.BLL.Interfaces
{
    public interface IEventService
    {
        IList<CEvent> GetCEvents();
    }
}
