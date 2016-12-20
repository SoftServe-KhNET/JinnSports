using System;
using System.Data.Entity;
using JinnSports.Entities.Entities;

namespace JinnSports.DAL.EFContext
{
    public class SportsDbInitializer : CreateDatabaseIfNotExists<SportsContext>
    {
        protected override void Seed(SportsContext context)
        {

            //    ---- Init sport types ----

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



            // ---- Init teams ----


            // Football teams
            Team mu = new Team()
            {
                Id = 1,
                Name = "Manchester United",
                SportType = football
            };
            Team milano = new Team()
            {
                Id = 2,
                Name = "Milano",
                SportType = football
            };
            Team mc = new Team()
            {
                Id = 3,
                Name = "Manchester City",
                SportType = football
            };
            Team chelsea = new Team()
            {
                Id = 4,
                Name = "Chelsea",
                SportType = football
            };
            Team bayern = new Team()
            {
                Id = 5,
                Name = "Bayern",
                SportType = football
            };

            // Basketball teams
            Team chicagoBulls = new Team()
            {
                Id = 6,
                Name = "Chicago Bulls",
                SportType = basketball
            };
            Team lalakers = new Team()
            {
                Id = 7,
                Name = "Los Angeles Lakers",
                SportType = basketball
            };
            Team phoenixSuns = new Team()
            {
                Id = 8,
                Name = "Phoenix Suns",
                SportType = basketball
            };

            //    --- Init Events --- 

            SportEvent chicagoBulls_vs_la_event = new SportEvent()
            {
                Id = 1,
                Date = new DateTime(2016, 11, 5, 16, 0, 0),
                SportType = basketball
            };
            SportEvent chicagoBulls_vs_suns_event = new SportEvent()
            {
                Id = 2,
                Date = new DateTime(2016, 11, 29, 16, 0, 0),
                SportType = basketball
            };
            SportEvent la_vs_suns_event = new SportEvent()
            {
                Id = 3,
                Date = new DateTime(2016, 11, 15, 16, 0, 0),
                SportType = basketball
            };

            SportEvent mu_vs_mc_event = new SportEvent()
            {
                Id = 5,
                Date = new DateTime(2016, 11, 19, 17, 0, 0),
                SportType = football
            };

            SportEvent bayern_vs_milano_event = new SportEvent()
            {
                Id = 6,
                Date = new DateTime(2016, 10, 28, 17, 0, 0),
                SportType = football
            };
            Result bayern_vs_milano = new Result()
            {
                Id = 9,
                Team = bayern,
                Score = 4,
                SportEvent = bayern_vs_milano_event
            };
            Result milano_vs_bayern = new Result()
            {
                Id = 10,
                Team = milano,
                Score = 1,
                SportEvent = bayern_vs_milano_event
            };


            SportEvent chelsea_vs_mc_event = new SportEvent()
            {
                Id = 7,
                Date = new DateTime(2016, 10, 17, 18, 0, 0),
                SportType = football
            };
            Result chelsea_vs_mc = new Result()
            {
                Id = 11,
                Team = chelsea,
                Score = 0,
                SportEvent = chelsea_vs_mc_event
            };
            Result mc_vs_chelsea = new Result()
            {
                Id = 12,
                Team = mc,
                Score = 0,
                SportEvent = chelsea_vs_mc_event
            };

            SportEvent chelsea_vs_milano_event = new SportEvent()
            {
                Id = 8,
                Date = new System.DateTime(2016, 11, 3, 16, 0, 0),
                SportType = football
            };
            Result chelsea_vs_milano = new Result()
            {
                Id = 13,
                Team = chelsea,
                Score = 2,
                SportEvent = chelsea_vs_milano_event
            };
            Result milano_vs_chelsea = new Result()
            {
                Id = 14,
                Team = milano,
                Score = 3,
                SportEvent = chelsea_vs_milano_event
            };


            // Init Results
            Result ch_vs_la = new Result()
            {
                Id = 1,
                Score = 68,
                Team = chicagoBulls,
                SportEvent = chicagoBulls_vs_la_event
            };
            Result la_vs_ch = new Result()
            {
                Id = 2,
                Score = 65,
                Team = lalakers,
                SportEvent = chicagoBulls_vs_la_event
            };

            Result la_vs_ph = new Result()
            {
                Id = 3,
                Score = 65,
                Team = lalakers,
                SportEvent = la_vs_suns_event
            };
            Result ph_vs_la = new Result()
            {
                Id = 4,
                Score = 64,
                Team = phoenixSuns,
                SportEvent = la_vs_suns_event
            };

            Result ch_vs_ph = new Result()
            {
                Id = 5,
                Score = 52,
                Team = chicagoBulls,
                SportEvent = chicagoBulls_vs_suns_event
            };
            Result ph_vs_ch = new Result()
            {
                Id = 6,
                Score = 52,
                Team = phoenixSuns,
                SportEvent = chicagoBulls_vs_suns_event
            };
            Result mu_vs_mc = new Result()
            {
                Id = 7,
                Score = 2,
                Team = mu,
                SportEvent = mu_vs_mc_event
            };
            Result mc_vs_mu = new Result()
            {
                Id = 8,
                Score = 1,
                Team = mc,
                SportEvent = mu_vs_mc_event
            };


            // Push entities to repo

            context.SportTypes.Add(football);
            context.SportTypes.Add(basketball);
            context.SportTypes.Add(tennis);
            context.SportTypes.Add(snooker);

            context.Results.Add(ch_vs_la);
            context.Results.Add(la_vs_ch);
            context.Results.Add(ch_vs_ph);
            context.Results.Add(ph_vs_ch);
            context.Results.Add(la_vs_ph);
            context.Results.Add(ph_vs_la);
            context.Results.Add(mu_vs_mc);
            context.Results.Add(mc_vs_mu);
            context.Results.Add(bayern_vs_milano);
            context.Results.Add(milano_vs_bayern);
            context.Results.Add(chelsea_vs_mc);
            context.Results.Add(mc_vs_chelsea);
            context.Results.Add(chelsea_vs_milano);
            context.Results.Add(milano_vs_chelsea);

            context.SportEvents.Add(chicagoBulls_vs_la_event);
            context.SportEvents.Add(chicagoBulls_vs_suns_event);
            context.SportEvents.Add(la_vs_suns_event);
            context.SportEvents.Add(bayern_vs_milano_event);
            context.SportEvents.Add(mu_vs_mc_event);
            context.SportEvents.Add(chelsea_vs_milano_event);
            context.SportEvents.Add(chelsea_vs_mc_event);

            context.Teams.Add(mu);
            context.Teams.Add(milano);
            context.Teams.Add(mc);
            context.Teams.Add(chelsea);
            context.Teams.Add(bayern);
            context.Teams.Add(chicagoBulls);
            context.Teams.Add(lalakers);
            context.Teams.Add(phoenixSuns);

            context.SaveChanges();
            base.Seed(context);
        }
    }
}
