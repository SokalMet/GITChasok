using Chasok4.DAL;
using Chasok4.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chasok4.Repositories
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private MessageRepository messageRepository;
        private UserRepository userRepository;
        private UserMessageRepository userMessageRepository;
              

        public MessageRepository Message
        {
            get
            {
                if (messageRepository == null)
                    messageRepository = new MessageRepository(db);
                return messageRepository;
            }
        }
        public UserRepository User
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }

        public UserMessageRepository UserMessage
        {
            get
            {
                if (userMessageRepository == null)
                    userMessageRepository = new UserMessageRepository(db);
                return userMessageRepository;
            }
        }


        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}