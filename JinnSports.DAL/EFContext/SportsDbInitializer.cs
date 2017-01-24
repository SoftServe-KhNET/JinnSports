using System.Data.Entity;
using JinnSports.Entities.Entities;

namespace JinnSports.DAL.EFContext
{
    public class SportsDbInitializer : CreateDatabaseIfNotExists<SportsContext>
    {
        protected override void Seed(SportsContext context)
        {            
            base.Seed(context);
        }
    }
}
