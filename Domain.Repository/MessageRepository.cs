using Data.Infrastructure;
using Data.Repository;
using Domain.Model;
using System;

namespace Domain.Repository
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(IDbFactory dbFactory) : base(dbFactory) { }
    }
}
