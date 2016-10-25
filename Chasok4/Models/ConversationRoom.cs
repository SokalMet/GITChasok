using Chasok4.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chasok4.Models
{
    public class ConversationRoom
    {
        [Key]
            public string RoomName { get; set; }
        public ConversationRoom()
        {
            Users = new List<string>();
        }
        public virtual List<string> Users { get; set; }
    }
}