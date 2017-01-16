using System;
using System.Threading.Tasks;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.DAL.EFContext;

namespace JinnSports.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly SportsContext db;

        private bool disposed = false;
       
        public EFUnitOfWork(SportsContext context)
        {
            this.db = context;            
        }                           

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new BaseRepository<T>(this.db);
        }

        public void SaveChanges()
        {
            this.db.SaveChanges();
        }

        public Task SaveAsync()
        {
            return this.db.SaveChangesAsync();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.db.Dispose();                    
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }        
    }
}
