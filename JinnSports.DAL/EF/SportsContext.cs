using System;
using System.Collections.Generic;
using System.Data.Entity;
using JinnSports.DAL.Entities;

namespace JinnSports.DAL.EF
{
    public class SportsContext : DbContext
    {
        public DbSet<CompetitionEvent> CompetitionEvents
        {
            get;
            set;
        }
        public DbSet<Result> Results
        {
            get;
            set;
        }
        public DbSet<SportType> SportTypes
        {
            get;
            set;
        }
        public DbSet<Team> Teams
        {
            get;
            set;
        }

        static SportsContext()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            Database.SetInitializer<SportsContext>(new StoreDbInitializer());
        }

        public SportsContext():base("name=SportsContext")
        {

        }
       
        public class StoreDbInitializer : DropCreateDatabaseIfModelChanges<SportsContext>
        {
            protected override void Seed(SportsContext db)
            {
                PutFootballEntities(db);
            }

            private void PutFootballEntities(SportsContext db)
            {
                SportType football = new SportType()
                {
                    Name = "Football"
                };
                db.SportTypes.Add(football);
                db.SportTypes.Add(new SportType()
                {
                    Name = "Basketball"
                });

                Team MC = new Team()
                {
                    Name = "MC",
                    SportType = football
                };
                Team MU = new Team()
                {
                    Name = "MU",
                    SportType = football
                };
                db.Teams.Add(MC);
                db.Teams.Add(MU);

                CompetitionEvent compEvent = new CompetitionEvent()
                {
                    Date = DateTime.Now
                };

                db.CompetitionEvents.Add(compEvent);
                db.Results.Add(new Result()
                {
                    Team = MC,
                    CompetitionEvent = compEvent,
                    Score = "3"
                });
                db.Results.Add(new Result()
                {
                    Team = MU,
                    CompetitionEvent = compEvent,
                    Score = "2"
                });
                db.SaveChanges();
            }
        }
    }
}