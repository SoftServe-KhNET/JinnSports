using JinnSports.DAL.Entities;
using JinnSports.DAL.Repositories;
using JinnSports.DataAccessInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.DAL
{
    class Test
    {
        static void Main(string[] args)
        {
            using (IStorage<CompetitionEvent> TestStorage = new Storage<CompetitionEvent>())
            {
                IRepository<CompetitionEvent> Rep = TestStorage.GetRepository();
                Console.WriteLine(Rep.GetAll().ToString());
                CompetitionEvent ciOne = new CompetitionEvent();
                ciOne.Date = DateTime.UtcNow;
                Rep.Add(ciOne);
                TestStorage.SaveChanges();
            }
        }
    }
}
