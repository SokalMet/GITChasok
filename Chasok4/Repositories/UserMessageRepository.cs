using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Chasok4.Models.Entities;
using Chasok4.DAL;

namespace Chasok4.Repositories
{
    public class UserMessageRepository : IUserMessageRepository
    {
        private ApplicationDbContext db;

        public UserMessageRepository()
        {
            this.db = new ApplicationDbContext();
        }
        public UserMessageRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void DeleteUserMessage(int userMessageId)
        {
            UserMessage userMessage = db.UserMessages.Find(userMessageId);
            if (userMessage != null)
                db.UserMessages.Remove(userMessage);
        }

        public UserMessage GetUserMessageById(int userMessageId)
        {
            return db.UserMessages.Find(userMessageId);
        }

        public IEnumerable<UserMessage> GetUserMessages()
        {
            return db.UserMessages.ToList();
        }

        public void AddUserMessage(UserMessage userMessage)
        {
            db.UserMessages.Add(userMessage);
        }

        public void UpdateUserMessage(UserMessage userMessage)
        {
            db.Entry(userMessage).State = System.Data.Entity.EntityState.Modified;
        }
    }
}