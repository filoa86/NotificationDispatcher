using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        NotificationDispatcherEntities dbContext;

        public NotificationDispatcherEntities Init()
        {
            return dbContext ?? (dbContext = new NotificationDispatcherEntities());
        }

        //Usato nei contesti in cui è necessaria un'istanza esclusiva (es. multithread)
        public NotificationDispatcherEntities NewInstance()
        {
            return new NotificationDispatcherEntities();
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }

        public new void Dispose()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
                dbContext = null;
            }

        }
    }

    public interface IDbFactory : IDisposable
    {
        NotificationDispatcherEntities Init();
        new void Dispose();
        NotificationDispatcherEntities NewInstance();
    }
}
