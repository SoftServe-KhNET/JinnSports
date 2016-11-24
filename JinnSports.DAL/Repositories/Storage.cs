using System;
using JinnSports.DataAccessInterfaces;
using JinnSports.DAL.Context;

namespace JinnSports.DAL.Repositories
{
    public class Storage<T> : IStorage<T> where T : class
    {
        private SportsContext<T> db;
        private BaseRepository<T> Repository;
       
        public Storage()
        {
            db = new SportsContext<T>();
        }

        public IRepository<T> GetRepository()
        {
            if (Repository == null)
            {
                Repository = new BaseRepository<T>(db.DbSet);
            }
            return Repository;
        }
        public void SaveChanges()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
