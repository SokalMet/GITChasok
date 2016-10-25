using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Chasok4.Models.Entities
{
    public class UserMessage
    {
        [Key]
        public int UserMessageId { get; set; }
        
        public DateTime DataTimeSend { get; set; }

        public DateTime DataTimeRead { get; set; }

        public virtual bool ReadStatus { get; set; } = false;


        public int MessageId { get; set; }
        [ForeignKey("MessageId")]
        public Message Message { get; set; }        
        
        //public string UserSendId { get; set; }
        //[ForeignKey("UserSendId")]        
        //public AppUser AppUserS { get; set; }

        //public List<string> UserReceiveId { get; set; }
        //[ForeignKey("UserReceiveId")]        
        //public AppUser AppUserR { get; set; }
    }
}