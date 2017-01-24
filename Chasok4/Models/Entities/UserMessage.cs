using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chasok4.Models.Entities
{
    public class UserMessage
    {
        public int Id { get; set; }
        public DateTime? ReadDate { get; set; }
        // public bool ReadStatus { get; set; } = false;

        public int MessageId { get; set; }
        public Message Message { get; set; } 
               
        public string ReceiverId { get; set; }
        public AppUser Receiver { get; set; }
     }
}