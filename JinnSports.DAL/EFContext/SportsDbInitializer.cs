using System;
using System.Data.Entity;
using JinnSports.Entities.Entities;

namespace JinnSports.DAL.EFContext
{
    public class SportsDbInitializer : CreateDatabaseIfNotExists<SportsContext>
    {
        /*protected override void Seed(SportsContext context)
        {
            /*
                ---- Init sport types ----
            
            SportType football = new SportType()
            {
                Id = 1,
                Name = "Football"
            };

            SportType basketball = new SportType()
            {
                Id = 2,
                Name = "Basketball"
            };

            SportType tennis = new SportType()
            {
                Id = 3,
                Name = "Tennis"
            };

            SportType snooker = new SportType()
            {
                Id = 4,
                Name = "Snooker"
            };


            /*
                ---- Init teams ----
            

            // Football teams
            Team MU = new Team()
            {
                Id = 1,
                Name = "Manchester United",
                SportType = football
            };
            Team Milano = new Team()
            {
                Id = 2,
                Name = "Milano",
                SportType = football
            };
            Team MC = new Team()
            {
                Id = 3,
                Name = "Manchester City",
                SportType = football
            };
            Team Chelsea = new Team()
            {
                Id = 4,
                Name = "Chelsea",
                SportType = football
            };
            Team Bayern = new Team()
            {
                Id = 5,
                Name = "Bayern",
                SportType = football
            };

            // Basketball teams
            Team ChicagoBulls = new Team()
            {
                Id = 6,
                Name = "Chicago Bulls",
                SportType = basketball
            };
            Team LALakers = new Team()
            {
                Id = 7,
                Name = "Los Angeles Lakers",
                SportType = basketball
            };
            Team PhoenixSuns = new Team()
            {
                Id = 8,
                Name = "Phoenix Suns",
                SportType = basketball
            };


            /*
                --- Init Events --- 
            
            SportEvent ChicagoBulls_vs_LA_event = new SportEvent()
            {
                Id = 1,
                Date = new DateTime(2016, 11, 5, 16, 0, 0)
            };
            SportEvent ChicagoBulls_vs_Suns_event = new SportEvent()
            {
                Id = 2,
                Date = new DateTime(2016, 11, 29, 16, 0, 0)
            };
            SportEvent LA_vs_Suns_event = new SportEvent()
            {
                Id = 3,
                Date = new DateTime(2016, 11, 15, 16, 0, 0)
            };
            SportEvent MU_vs_Bayern_event = new SportEvent()
            {
                Id = 4,
                Date = new DateTime(2016, 12, 6, 15, 0, 0)
            };
            SportEvent MU_vs_MC_event = new SportEvent()
            {
                Id = 5,
                Date = new DateTime(2016, 11, 19, 17, 0, 0)
            };

            SportEvent Bayern_vs_Milano_event = new SportEvent()
            {
                Id = 6,
                Date = new DateTime(2016, 10, 28, 17, 0, 0)
            };
            Result Bayern_vs_Milano = new Result()
            {
                Id = 9,
                Team = Bayern,
                Score = 4,
                SportEvent = Bayern_vs_Milano_event
            };
            Result Milano_vs_Bayern = new Result()
            {
                Id = 10,
                Team = Milano,
                Score = 1,
                SportEvent = Bayern_vs_Milano_event
            };


            SportEvent Chelsea_vs_MC_event = new SportEvent()
            {
                Id = 7,
                Date = new DateTime(2016, 10, 17, 18, 0, 0)
            };
            Result Chelsea_vs_MC = new Result()
            {
                Id = 11,
                Team = Chelsea,
                Score = 0,
                SportEvent = Chelsea_vs_MC_event
            };
            Result MC_vs_Chelsea = new Result()
            {
                Id = 12,
                Team = MC,
                Score = 0,
                SportEvent = Chelsea_vs_MC_event
            };

            SportEvent Chelsea_vs_Milano_event = new SportEvent()
            {
                Id = 8,
                Date = new System.DateTime(2016, 11, 3, 16, 0, 0)
            };
            Result Chelsea_vs_Milano = new Result()
            {
                Id = 13,
                Team = Chelsea,
                Score = 2,
                SportEvent = Chelsea_vs_Milano_event
            };
            Result Milano_vs_Chelsea = new Result()
            {
                Id = 14,
                Team = Milano,
                Score = 3,
                SportEvent = Chelsea_vs_Milano_event
            };


            // Init Results
            Result Ch_vs_LA = new Result()
            {
                Id = 1,
                Score = 68,
                Team = ChicagoBulls,
                SportEvent = ChicagoBulls_vs_LA_event
            };
            Result LA_vs_Ch = new Result()
            {
                Id = 2,
                Score = 65,
                Team = LALakers,
                SportEvent = ChicagoBulls_vs_LA_event
            };

            Result LA_vs_Ph = new Result()
            {
                Id = 3,
                Score = 65,
                Team = LALakers,
                SportEvent = LA_vs_Suns_event
            };
            Result Ph_vs_LA = new Result()
            {
                Id = 4,
                Score = 64,
                Team = PhoenixSuns,
                SportEvent = LA_vs_Suns_event
            };

            Result Ch_vs_Ph = new Result()
            {
                Id = 5,
                Score = 52,
                Team = ChicagoBulls,
                SportEvent = ChicagoBulls_vs_Suns_event
            };
            Result Ph_vs_Ch = new Result()
            {
                Id = 6,
                Score = 52,
                Team = PhoenixSuns,
                SportEvent = ChicagoBulls_vs_Suns_event
            };
            Result MU_vs_MC = new Result()
            {
                Id = 7,
                Score = 2,
                Team = MU,
                SportEvent = MU_vs_MC_event
            };
            Result MC_vs_MU = new Result()
            {
                Id = 8,
                Score = 1,
                Team = MC,
                SportEvent = MU_vs_MC_event
            };


            // Push entities to repo

            context.SportTypes.Add(football);
            context.SportTypes.Add(basketball);
            context.SportTypes.Add(tennis);
            context.SportTypes.Add(snooker);


            context.Teams.Add(MU);
            context.Teams.Add(Milano);
            context.Teams.Add(MC);
            context.Teams.Add(Chelsea);
            context.Teams.Add(Bayern);
            context.Teams.Add(ChicagoBulls);
            context.Teams.Add(LALakers);
            context.Teams.Add(PhoenixSuns);

            context.SportEvents.Add(ChicagoBulls_vs_LA_event);
            context.SportEvents.Add(ChicagoBulls_vs_Suns_event);
            context.SportEvents.Add(LA_vs_Suns_event);
            context.SportEvents.Add(MU_vs_Bayern_event);
            context.SportEvents.Add(Bayern_vs_Milano_event);
            context.SportEvents.Add(MU_vs_MC_event);
            context.SportEvents.Add(Chelsea_vs_Milano_event);
            context.SportEvents.Add(Chelsea_vs_MC_event);

            context.Results.Add(Ch_vs_LA);
            context.Results.Add(LA_vs_Ch);
            context.Results.Add(Ch_vs_Ph);
            context.Results.Add(Ph_vs_Ch);
            context.Results.Add(LA_vs_Ph);
            context.Results.Add(Ph_vs_LA);
            context.Results.Add(MU_vs_MC);
            context.Results.Add(MC_vs_MU);
            context.Results.Add(Bayern_vs_Milano);
            context.Results.Add(Milano_vs_Bayern);
            context.Results.Add(Chelsea_vs_MC);
            context.Results.Add(MC_vs_Chelsea);
            context.Results.Add(Chelsea_vs_Milano);
            context.Results.Add(Milano_vs_Chelsea);

            context.SaveChanges();
            base.Seed(context);
        }*/
    }
}
