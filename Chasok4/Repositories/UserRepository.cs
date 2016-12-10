using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Chasok4.Models.Entities;
using Chasok4.Models;
using System.Xml.Linq;
using Chasok4.DAL;
using Chasok4.Projections;

namespace Chasok4.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext db;

        public UserRepository()
        {
            this.db = new ApplicationDbContext();
        }                  

        public UserRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void DeleteUser(string userId)
        {
            AppUser user = db.Users.Find(userId);
            if (user!=null)
            db.Users.Remove(user); 
        }

        public AppUser GetUserById(string userId)
        {
            return db.Users.Find(userId);
        }

        public AppUser GetUserByName(string name)
        {
            return db.Users.Where(x=>x.UserName == name).FirstOrDefault();
        }

        public List<ProjectionUsers> UsersList()
        {
            return db.Users.Select(x=> new ProjectionUsers
            {
                UserId = x.Id,
                UserEmail = x.Email
            }).ToList();
        }

        public IEnumerable<AppUser> GetUsers()
        {
            return db.Users.ToList();
        }

        public void InsertUser(AppUser user)
        {
            db.Users.Add(user);
        }
        
        public void UpdateUser(AppUser user)
        {
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
        }
    }
}