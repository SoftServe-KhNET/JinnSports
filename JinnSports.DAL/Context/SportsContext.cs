using System;
using System.Collections.Generic;
using System.Data.Entity;
using JinnSports.DAL.Entities;

namespace JinnSports.DAL.Context
{
    public class SportsContext<T> : DbContext where T : class
    {
        public DbSet<T> DbSet;

        static SportsContext()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
        }

        public SportsContext() : base("LocalMsSql")
        {
           
        }

    }
}