using System;
using JinnSports.DataAccessInterfaces;
using JinnSports.DAL.EF;

namespace JinnSports.DAL.Repositories
{
    public class EFUnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        private SportsContext db;
        private BaseRepository<T> baseRepository;
       
        public EFUnitOfWork()
        {
            db = new SportsContext();
        }

        public IRepository<T> GetRepository()
        {
            if (baseRepository == null)
            {
                baseRepository = new BaseRepository<T>(db);
            }
            return baseRepository;
        }
        public void Save()
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
