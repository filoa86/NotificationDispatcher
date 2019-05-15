using Data.Repository;
using System;

namespace Domain.Service
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
    }

    public interface IMessageService
    {
    }
}
