using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chasok4.Models.Entities
{
    public class UserMessage
    {
        public int UserMessageId { get; set; }
        
        public int MessageId { get; set; }
        public virtual Message Message { get; set; }
        
        public string Id { get; set; }
        public virtual AppUser User { get; set; }

        public virtual bool ReadStatus { get; set; } = false;

    }
}