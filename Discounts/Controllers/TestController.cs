using System;
using Discounts.Models;
using System.Web.Mvc;

namespace Discounts.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DeleteInfo()
        {
            using (var db = new DB())
            {
                foreach (var image in db.Images)
                {
                    try
                    {
                        System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Images/") + image.Guid + image.Extension);
                    }
                    catch (Exception e)
                    {

                    }
                }
                db.Images.RemoveRange(db.Images);
                db.Brands.RemoveRange(db.Brands);
                db.SaveChanges();
            }
            return null;
        }
    }
}






































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