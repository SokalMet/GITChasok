﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chasok4.Models.Entities
{
    public class UserMessage
    {
        public int Id { get; set; }
        public bool ReadStatus { get; set; } = false;

        public AppUser Creator { get; set; }

        public string ReceiverId { get; set; }
        public IList<AppUser> receiver { get; set; }        
    }
}