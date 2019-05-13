using Microsoft.EntityFrameworkCore;
using System;

namespace Data
{
    public class NotificationDispatcherEntities : DbContext
    {
        // "name=dbNotificationDispatcher"
        public NotificationDispatcherEntities() : base() { }
    }
}
