using Chasok4.DAL;
using Chasok4.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chasok4.Repositories
{
    public class MessageRepository:IMessageRepository
    {
        private ApplicationDbContext db;

        public MessageRepository()
        {
            this.db = new ApplicationDbContext();
        }
        public MessageRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void DeleteMessage(int messageId)
        {
            Message message = db.Messages.Find(messageId);
            if (message!=null)
            db.Messages.Remove(message);
        }

        public Message GetMessageById(int messageId)
        {
            return db.Messages.Find(messageId);
        }

        public IEnumerable<Message> GetMessages()
        {
            return db.Messages.ToList();
        }

        public IEnumerable<Message> GetMessages(IEnumerable<UserMessage> usersMessages)
        {      
            List<Message> MessList = db.Messages.ToList();     
            return MessList.Where(x => (usersMessages.Select(y=>y.MessageId).Contains(x.Id)));
        }

        public void AddMessage(Message message)
        {
            db.Messages.Add(message);
        }
       
        public void UpdateMessage(Message message)
        {
            db.Entry(message).State = System.Data.Entity.EntityState.Modified;
        }
    }
}