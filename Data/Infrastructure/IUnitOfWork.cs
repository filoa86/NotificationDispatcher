using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Data.Infrastructure
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        int SaveChangesAndDispose();
        bool HasActiveTransaction();
        void BeginTransaction();
        void BeginTransaction(IsolationLevel isolationLevel);
        void Commit();
        void RollBack();
        Task<int> SaveChangesAsync();
        void DisposeDbContext();
    }
}
