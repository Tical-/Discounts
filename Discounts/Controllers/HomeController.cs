using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Discounts.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Discounts.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public JsonResult Test()
        {
            Thread.Sleep(5000);
            return Json("");
        }
        public ActionResult Index()
        {
            //using (var db = new ApplicationDbContext())
            //{
            //    if (!db.Roles.Any(r => r.Name == "Administrator"))
            //    {
            //        var store = new RoleStore<IdentityRole>(db);
            //        var manager = new RoleManager<IdentityRole>(store);
            //        var role = new IdentityRole { Name = "Administrator" };
            //        manager.Create(role);
            //    }
            //}


            //var user = new ApplicationUser
            //{
            //    UserName = "sanjars",
            //    Email = "sanjars@live.com"
            //};
            //string password = "NEWwave320011";

            //using (var db = new ApplicationDbContext())
            //{
            //    var store = new UserStore<ApplicationUser>(db);
            //    var manager = new UserManager<ApplicationUser, string>(store);

            //    var result = manager.Create(user, password);
            //    if (!result.Succeeded)
            //        throw new ApplicationException("Unable to create a user.");

            //    result = manager.AddToRole(user.Id, "Administrator");
            //    if (!result.Succeeded)
            //        throw new ApplicationException("Unable to add user to a role.");
            //}

            return View();
        }
    }
}
