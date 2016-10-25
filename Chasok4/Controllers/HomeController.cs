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
        UnitOfWork unitOfWork;
        
        public HomeController()
        {
            unitOfWork = new UnitOfWork();
        }

        public ActionResult Index()
        {
            var users = unitOfWork.User.GetUsers();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Chat()
        {
            var myUser = unitOfWork.User.GetUsers();
            //IEnumerable<ApplicationUser> allUsers = userRep.GetUsers();
            ViewBag.UpperTitle = "Your Chat page.";
            //ViewBag.UserName = unitOfWork.User.User
            //ViewBag.ListOfMessages = unitOfWork.Message.GetMessages() ;

            ViewBag.getAllUsers = new SelectList(GetUsersFriends(), "Value", "Text");
            ViewBag.userId = unitOfWork.User.GetUserByName(User.Identity.Name).Id;
            return View(myUser);
        }



        [HttpPost]
        public void MessageAction(Message message)
        {
            unitOfWork.Message.AddMessage(message);
        }


        #region Custom methods

        [NonAction]
        public List<SelectListItem> GetUsersFriends()
        {
            var sourceItems = new List<SelectListItem>();
            
            foreach (var item in unitOfWork.User.GetUsers().Where(x => x.UserName!=User.Identity.Name).OrderBy(x => x.UserName)
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