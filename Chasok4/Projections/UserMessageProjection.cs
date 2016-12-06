using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chasok4.Projections
{
    public class UserMessageProjection
    {
        public int Id { get; set; }
        
        public string ReceiverId { get; set; }
    }
}