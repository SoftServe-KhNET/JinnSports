using System.Collections.Generic;
using JinnSports.Entities.Entities;

namespace JinnSports.BLL.Interfaces
{
    public interface IResultService : IService
    {
        void SaveResults(List<Result> results);
    }
}
