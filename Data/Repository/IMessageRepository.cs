using Data.Infrastructure;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public interface IMessageRepository : IRepository<Message>
    {
    }
}
