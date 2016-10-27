using Chasok4.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chasok4.Repositories
{
    interface IUserMessageRepository
    {
        IEnumerable<UserMessage> GetUserMessages();
        UserMessage GetUserMessageById(int userMessageId);
        void AddUserMessage(UserMessage userMessage);
        void DeleteUserMessage(int userMessageId);
        void UpdateUserMessage(UserMessage userMessage);
    }
}
