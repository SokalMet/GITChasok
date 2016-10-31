using Chasok4.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chasok4.Repositories
{
    interface IMessageRepository
    {
        IEnumerable<Message> GetMessages();
        Message GetMessageById(int messageId);
        void AddMessage(Message message);
        void DeleteMessage(int messageId);
        void UpdateMessage(Message message);
        
    }
}
