﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Chasok4.Models.Entities;
using Chasok4.Models;
using System.Xml.Linq;
using Chasok4.DAL;

namespace Chasok4.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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

            db.Users.Remove(user);
        }

        public AppUser GetUserById(string userId)
        {
            return db.Users.Find(userId);
        }

        public IEnumerable<AppUser> GetUsers()
        {
            return db.Users.ToList();
        }

        public void InsertUser(AppUser user)
        {
            db.Users.Add(user);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void UpdateUser(AppUser user)
        {
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
        }
    }
}