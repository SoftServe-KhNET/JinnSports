namespace JinnSports.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<JinnSports.DAL.EFContext.SportsContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
            this.AutomaticMigrationDataLossAllowed = false;
            this.ContextKey = "JinnSports.DAL.EFContext.SportsContext";
        }

        protected override void Seed(JinnSports.DAL.EFContext.SportsContext context)
        {

        }
    }
}
