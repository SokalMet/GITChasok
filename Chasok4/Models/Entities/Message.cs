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
            userMessages = new HashSet<UserMessage>();
        }
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ReadDate { get; set; }

        public string CreatorId { get; set; }
        public AppUser Creator { get; set; }
        
        public virtual ICollection<UserMessage> userMessages { get; set; }
    }
}