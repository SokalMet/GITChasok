using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chasok4.Models
{
    public class MyMessage
    {
        public string Msg { get; set; }
        public string Group { get; set; }
        public List<string> SelectedUsers { get; set; }
    }
}