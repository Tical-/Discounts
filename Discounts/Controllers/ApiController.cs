using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Discounts.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Discounts.Controllers
{
    [Authorize(Roles = "Administrator")]
    [RoutePrefix("api/AdminApi")]
    public class AdminApiController : ApiController
    {

        [HttpPost]
        public List<UsersModel.AdmUser> GetUsers()
        {
            using (var db = new ApplicationDbContext())
            {
                var store = new UserStore<ApplicationUser>(db);
                var manager = new UserManager<ApplicationUser, string>(store);
                var RoleStore = new RoleStore<IdentityRole>(db);
                var RoleManager = new RoleManager<IdentityRole>(RoleStore);
                var admRole = RoleManager.FindByName("Administrator");
                return manager.Users.Select(z => new UsersModel.AdmUser
                {
                    Id = z.Id,
                    Email = z.Email,
                    UserName = z.UserName,
                    IsAdmin = z.Roles.Select(u => u.RoleId).Contains(admRole.Id)
                })
                    .ToList();
            }
        }

        [HttpPost]
        public void SaveUsers([FromBody] UsersModel model)
        {
            using (var db = new ApplicationDbContext())
            {
                var RoleStore = new RoleStore<IdentityRole>(db);
                var RoleManager = new RoleManager<IdentityRole>(RoleStore);
                var admRole = RoleManager.FindByName("Administrator");
                var store = new UserStore<ApplicationUser>(db);
                var manager = new UserManager<ApplicationUser, string>(store);
                //DELETE
                var Emails = model.Users.Select(u => u.Email).ToList();
                var users = manager.Users.Where(z => !Emails.Contains(z.Email)).ToList();
                foreach (var applicationUser in users)
                {
                    var f = manager.Delete(applicationUser);
                    if (!f.Succeeded)
                        throw new Exception(string.Join(", ", f.Errors));
                }
                foreach (var item in model.Users)
                {
                    var user = manager.Users.FirstOrDefault(z => z.Email == item.Email);
                    if (user == null) //INSERT
                    {
                        var newuser = new ApplicationUser
                        {
                            UserName = item.UserName,
                            Email = item.Email
                        };
                        string password = "123123";
                        var result = manager.Create(newuser, password);
                        if (!result.Succeeded)
                            throw new Exception(string.Join(", ", result.Errors));
                        if (item.IsAdmin && !manager.IsInRoleAsync(user.Id, "Administrator").Result)
                        {
                            var r = manager.AddToRole(user.Id, "Administrator");
                            if (!r.Succeeded)
                                throw new Exception(string.Join(", ", r.Errors));
                        }
                    }
                    else //UPDATE
                    {
                        user.UserName = item.UserName;
                        if (item.IsAdmin && !manager.IsInRoleAsync(user.Id, "Administrator").Result)
                        {
                            var r = manager.AddToRole(user.Id, "Administrator");
                            if (!r.Succeeded)
                                throw new Exception(string.Join(", ", r.Errors));
                        }
                        else
                        {
                            if (!item.IsAdmin)
                            {
                                var s = manager.RemoveFromRole(user.Id, "Administrator");
                                if (!s.Succeeded)
                                    throw new Exception(string.Join(", ", s.Errors));
                            }
                        }
                    }
                }
            }
        }
    }
}
