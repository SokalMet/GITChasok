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
            AllUsersMessageTo = new List<AppUser>();
            ReadStatus = false;
        }
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreateData { get; set; }
        public DateTime? ReadData { get; set; }

        public bool ReadStatus { get; set; }
        public virtual List<AppUser> AllUsersMessageTo { get; set; }

        public virtual AppUser Creator { get; set; }

    }
}