using Data.Extension;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Data.Infrastructure
{
    public class UnitOfWork : Disposable, IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        // private MotusEntities _dbContext;
        private IDbContextTransaction _transaction;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        protected override void DisposeCore()
        {
            _transaction?.Dispose();
        }

        public NotificationDispatcherEntities DbContext
        {
            get { return dbFactory.Init(); }
        }

        public void BeginTransaction()
        {
            BeginTransaction(System.Data.IsolationLevel.Snapshot);
        }
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            _transaction = DbContext.Database.BeginTransaction(isolationLevel);
        }

        public void Commit()
        {
            try
            {
                CommitTransaction();
            }
            catch
            {
                RollBack();
                throw;
            }
        }

        private void CommitTransaction()
        {
            DbContext.SaveChanges();
            _transaction?.Commit();
        }

        public bool HasActiveTransaction()
        {
            return DbContext.Database.CurrentTransaction != null;
        }

        public void RollBack()
        {
            DbContext.Reset();

            if (_transaction != null && _transaction.GetDbTransaction() != null && _transaction.TransactionId != null)
                _transaction?.Rollback();

        }

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        public void DisposeDbContext()
        {
            dbFactory.Dispose();
        }

        public int SaveChangesAndDispose()
        {
            int result = this.SaveChanges();
            this.DisposeDbContext();
            return result;
        }
    }
}
