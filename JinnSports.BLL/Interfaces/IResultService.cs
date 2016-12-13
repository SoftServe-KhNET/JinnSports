using System.Collections.Generic;
using JinnSports.Entities.Entities;

namespace JinnSports.BLL.Interfaces
{
    public interface IResultService
    {
        void SaveResults(List<Result> results);
    }
}
