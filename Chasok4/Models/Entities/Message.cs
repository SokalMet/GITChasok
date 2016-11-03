using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chasok4.Models.Entities
{
    public class Message
    {
        public Message()
        {            
            this.UserMessage = new List<UserMessage>();
        }
        public int MessageId { get; set; }
        public string Body { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual AppUser CreaterId { get; set; }

        public int UserId { get; set; }
        public virtual AppUser User { get; set; }

        public virtual List<UserMessage> UserMessage { get; set; }
    }
}