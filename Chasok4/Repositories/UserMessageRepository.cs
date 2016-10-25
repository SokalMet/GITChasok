using Chasok4.DAL;
using Chasok4.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chasok4.Repositories
{
    public class UserMessageRepository
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

        public void DeleteUserMessage(int usermessageId)
        {
            UserMessage usermessage = db.UserMessages.Find(usermessageId);
            if(usermessage!=null)
            db.UserMessages.Remove(usermessage);
        }

        public UserMessage GetUserMessageById(int usermessageId)
        {
            return db.UserMessages.Find(usermessageId);
        }

        public IEnumerable<UserMessage> GetUserMessages()
        {
            return db.UserMessages.ToList();
        }

        public void AddUserMessage(UserMessage usermessage)
        {
            db.UserMessages.Add(usermessage);
        }
       

        public void UpdateUserMessage(UserMessage usermessage)
        {
            db.Entry(usermessage).State = System.Data.Entity.EntityState.Modified;
        }
    }
}