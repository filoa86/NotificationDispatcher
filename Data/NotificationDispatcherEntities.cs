using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Data
{
    public class NotificationDispatcherEntities : DbContext
    {
        // "name=dbNotificationDispatcher"
        public NotificationDispatcherEntities() : base() { }

        public DbSet<Message> Messages { get; set; }

        public new virtual int SaveChanges()
        {
            return base.SaveChanges();
        }
        public virtual Task<int> CommitAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
