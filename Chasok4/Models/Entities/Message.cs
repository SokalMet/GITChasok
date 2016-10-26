using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chasok4.Models.Entities
{
    public class Message
    {       
        public int Id { get; set; }
        public string Body { get; set; }
        public string RoomName { get; set; }
        public List<string> Friends { get; set; }
    }
}