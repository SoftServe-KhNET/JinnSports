﻿using System.Collections.Generic;
using JinnSports.Entities.Entities;

namespace JinnSports.Parser.App.Interfaces
{
    public interface ISaver
    {
        void DBSaveChanges(List<Result> results);
    }
}