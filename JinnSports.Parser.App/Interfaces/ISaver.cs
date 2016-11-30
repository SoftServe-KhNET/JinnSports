using System;
using System.Collections.Generic;
using JinnSports.DAL.Entities;

namespace JinnSports.Parser.App.Interfaces
{
    public interface ISaver
    {
        void DBSaveChanges(List<Result> results);
    }
}
