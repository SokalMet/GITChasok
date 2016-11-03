using Chasok4.Models.Entities;
using Chasok4.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chasok4.Controllers
{
    public class HomeController : Controller
    {
        IUserRepository db = new UserRepository();
        
        public HomeController()
        {
            this.db = new UserRepository();          
        }

        public ActionResult Index()
        {
            return View(db.GetUsers());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Chat()
        {
            var Users = db.GetUsers();
            //IEnumerable<ApplicationUser> allUsers = userRep.GetUsers();
            ViewBag.Message = "Your Chat page.";
            //ViewBag.ListOfUsers = allUsers;
            return View(Users);
        }

        
        
    }
}