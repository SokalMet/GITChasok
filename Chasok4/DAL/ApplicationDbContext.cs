using Chasok4.Models.Entities;
using Chasok4.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Chasok4.DAL
{  
        public class ApplicationDbContext : IdentityDbContext<AppUser>
        {
        public DbSet<IdentityUser> AppUsers { get; set; }

        public ApplicationDbContext()
                : base("ConnectionToChasok")//, throwIfV1Schema: false)
            {
            }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public DbSet<IdentityUser> AppUser { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserMessage> MessagesForUsers { get; set; }

        
    }   
}