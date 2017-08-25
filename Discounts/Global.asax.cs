using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Discounts.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Discounts
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            CreateSA();
        }

        protected void CreateSA()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    if (!db.Roles.Any(r => r.Name == "Administrator"))
                    {
                        var store = new RoleStore<IdentityRole>(db);
                        var manager = new RoleManager<IdentityRole>(store);
                        var role = new IdentityRole { Name = "Administrator" };
                        manager.Create(role);
                    }
                }
                var user = new ApplicationUser
                {
                    UserName = "sanjars",
                    Email = "sanjars@live.com"
                };
                string password = "NEWwave320011";
                using (var db = new ApplicationDbContext())
                {
                    var store = new UserStore<ApplicationUser>(db);
                    var manager = new UserManager<ApplicationUser, string>(store);
                    var result = manager.Create(user, password);
                    //if (!result.Succeeded)
                        //throw new ApplicationException("Unable to create a user.");
                    result = manager.AddToRole(user.Id, "Administrator");
                    //if (!result.Succeeded)
                        //throw new ApplicationException("Unable to add user to a role.");
                }
            }
            catch (Exception e)
            {
                //throw;
            }
        }
    }
}
