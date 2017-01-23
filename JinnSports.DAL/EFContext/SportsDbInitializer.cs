using System;
using System.Data.Entity;
using JinnSports.Entities.Entities;
using JinnSports.Entities.Entities.Temp;
using System.Collections.Generic;

namespace JinnSports.DAL.EFContext
{
    public class SportsDbInitializer : CreateDatabaseIfNotExists<SportsContext>
    {
        protected override void Seed(SportsContext context)
        {
            Conformity c1 = new Conformity { InputName = "Динамо", ExistedName = "Динамо З" };
            Conformity c2 = new Conformity { InputName = "Динамо", ExistedName = "Динамо К" };
            Conformity c3 = new Conformity { InputName = "Динамо", ExistedName = "Динамо Б" };
            Conformity c4 = new Conformity { InputName = "Сент-Этьен", ExistedName = "Ст. Этьен" };

            TempResult tr1 = new TempResult { Score = 0, IsHome = true, Conformities = new List<Conformity> { c1, c2, c3 } };
            TempResult tr2 = new TempResult { Score = 1, IsHome = false, Conformities = new List<Conformity> { c4 } };

            SportType sport = new SportType { Name = "MySport" };

            TempSportEvent ev = new TempSportEvent { Date = DateTime.Now, SportType = sport, TempResults = new List<TempResult> { tr1, tr2 } };

            context.TempSportEvents.Add(ev);

            TeamName tn1 = new TeamName { Name = "Металлист" };
            Team t1 = new Team { Name = "Металлист", Names = new List<TeamName> { tn1 }, SportType = sport };

            TeamName tn2 = new TeamName { Name = "Динамо З" };
            Team t2 = new Team { Name = "Динамо З", Names = new List<TeamName> { tn2 }, SportType = sport };

            TeamName tn3 = new TeamName { Name = "Динамо К" };
            Team t3 = new Team { Name = "Динамо К", Names = new List<TeamName> { tn3 }, SportType = sport };

            TeamName tn4 = new TeamName { Name = "Динамо Б" };
            Team t4 = new Team { Name = "Динамо Б", Names = new List<TeamName> { tn4 }, SportType = sport };

            TeamName tn5 = new TeamName { Name = "Ст. Этьен" };
            Team t5 = new Team { Name = "Ст. Этьен", Names = new List<TeamName> { tn5 }, SportType = sport };

            context.Teams.Add(t1);
            context.Teams.Add(t2);
            context.Teams.Add(t3);
            context.Teams.Add(t4);
            context.Teams.Add(t5);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
