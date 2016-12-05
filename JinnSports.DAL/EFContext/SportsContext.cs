using System;
using System.Collections.Generic;
using System.Data.Entity;
using JinnSports.Entities;

namespace JinnSports.DAL.EFContext
{
    public class SportsContext : DbContext
    {
        public SportsContext() : base("SqlServerConnection")
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
        }

        public DbSet<CompetitionEvent> CompetitionEvents
        {
            get; set;
        }
        public DbSet<Result> Results
        {
            get; set;
        }
        public DbSet<SportType> SportTypes
        {
            get; set;
        }
        public DbSet<Team> Teams
        {
            get; set;
        }
       
        
    }
}