using Chasok4.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chasok4.Repositories
{
    public class MessageRepository
    {
        DAL.ApplicationDbContext db;

        public MessageRepository(DAL.ApplicationDbContext db)
        {
            this.db = db;
        }
        public Message GetMessageById(int messageId)
        {
            return db.Messages.Find(messageId);
        }
        public IEnumerable<Message> GetMessages()
        {
            return db.Messages.ToList();
        }
        public void AddMessage(Message message)
        {
            db.Messages.Add(message);
        }
    }
}