using Chasok4.Models.Entities;
using Chasok4.Repositories;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chasok4.Controllers
{
    public class HomeController : Controller
    {
        IUnitOfWork uW;
        
        public HomeController(IUnitOfWork u)
        {
            uW=u;
        }

        public ActionResult Index()
        {
            
            return View(uW.User);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Chat()
        {
            var myUser = uW.User.GetUsers();
            ViewBag.UpperTitle = "Your Chat page.";

            ViewBag.getAllUsers = new SelectList(GetUsersFriends(), "Value", "Text");
            ViewBag.userId = uW.User.GetUserByName(User.Identity.Name).Id;
            
            return View(myUser);
        }

        #region Custom methods

        [NonAction]
        public List<SelectListItem> GetUsersFriends()
        {
            var sourceItems = new List<SelectListItem>();
            
            foreach (var item in uW.User.GetUsers().Where(x => x.UserName!=User.Identity.Name).OrderBy(x => x.UserName)
                .Select(x => x.Email).ToList())
            {
                sourceItems.Add(new SelectListItem()
                {
                    Value = item,
                    Text = item                    
                });
            }

            return sourceItems;
        }

        #endregion
    }
}