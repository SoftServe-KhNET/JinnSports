using System;
using JinnSports.DAL.EFContext;
using System.Data.Entity;
using JinnSports.Interfaces.DALInterfaces;
using System.Collections.Generic;

namespace JinnSports.DAL.Utility
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private SportEntities dbContext;
        public EFUnitOfWork()
        {
            dbContext = new SportEntities();
        }

        public DbSet<T> Set<T>() where T : class
        {
            return dbContext.Set<T>();
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
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
