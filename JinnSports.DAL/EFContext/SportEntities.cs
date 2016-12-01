using System;
using System.Collections.Generic;
using System.Data.Entity;
using JinnSports.DAL.Entities;

namespace JinnSports.DAL.EFContext
{
    public class SportEntities : DbContext
    {
        public DbSet<CompetitionEvent> CompetitionEvents;
        public DbSet<Result> Results;
        public DbSet<SportType> SportTypes;
        public DbSet<Team> Teams;
       
        public SportEntities() : base("SqlServerConnection")
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
        }
    }
}