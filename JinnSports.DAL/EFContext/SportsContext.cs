using System.Configuration;
using System.Data.Entity;
using JinnSports.Entities.Entities;
using JinnSports.Entities.Entities.Temp;
using JinnSports.Entities.Entities.Identity;
using JinnSports.DAL.Configurations;

namespace JinnSports.DAL.EFContext
{
    public class SportsContext : DbContext
    {
        public SportsContext(string connectionName) : base(GetConnectionString(connectionName))
        {
            Database.SetInitializer(new SportsDbInitializer());
        }

        public SportsContext() : base("SportsContext")
        {

        }

        public DbSet<SportEvent> SportEvents { get; set; }

        public DbSet<Result> Results { get; set; }

        public DbSet<SportType> SportTypes { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamName> TeamNames { get; set; }

        public DbSet<EventPrediction> EventPredictions { get; set; }

        public DbSet<Conformity> Conformities { get; set; }

        public DbSet<TempResult> TempResults { get; set; }

        public DbSet<TempSportEvent> TempSportEvents { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }        

        public DbSet<ExternalLogin> Logins { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Result>().HasRequired(p => p.Team).WithMany(n => n.Results).WillCascadeOnDelete(false);
            modelBuilder.Entity<Result>().HasRequired(p => p.SportEvent).WithMany(n => n.Results).WillCascadeOnDelete(false);

            modelBuilder.Entity<Team>().HasRequired(p => p.SportType).WithMany(n => n.Teams).WillCascadeOnDelete(false);
            modelBuilder.Entity<Team>().Property(p => p.Name).IsRequired();            

            modelBuilder.Entity<SportType>().Property(p => p.Name).IsRequired();

            modelBuilder.Entity<SportEvent>().HasRequired(p => p.SportType).WithMany(n => n.SportEvents).WillCascadeOnDelete(false);

            modelBuilder.Entity<TeamName>().Property(p => p.Name).IsRequired();

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new ExternalLoginConfiguration());            

            base.OnModelCreating(modelBuilder);
        }

        private static string GetConnectionString(string connectionName)
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }
    }
}