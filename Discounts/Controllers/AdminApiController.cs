using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Discounts.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebGrease.Css.Extensions;

namespace Discounts.Controllers
{
    [System.Web.Http.Authorize(Roles = "Administrator")]
    [System.Web.Http.RoutePrefix("api/AdminApi")]
    public class AdminApiController : ApiController
    {
        [System.Web.Http.HttpPost]
        public int? InsertStore(Store store)
        {
            using (var db = new DB())
            {
                if (store == null)
                    throw new Exception("Store NULL exception");
                var st = new Stores()
                {
                    Name = store.Name,
                    Description = store.Description
                };
                db.Stores.Add(st);
                db.SaveChanges();
                db.Images.First(z => z.Id == store.ImageId).StoreId = st.Id;
                db.SaveChanges();
                return st.Id;
            }
        }


        [System.Web.Http.HttpPost]
        public Brand StoreGetById(int Id)
        {
            using (var db = new DB())
            {
                var brand = db.Brands.First(z => z.Id == Id);
                return new Brand() { Id = brand.Id, Name = brand.Name, Description = brand.Description, File = brand.Images.First().Guid + brand.Images.First().Extension };
            }
        }

        [System.Web.Http.HttpPost]
        public void DeleteBrand(int Id)
        {
            using (var db = new DB())
            {
                var brand = db.Brands.FirstOrDefault(z => z.Id == Id);
                if (brand != null)
                {
                    foreach (var image in brand.Images)
                    {
                        File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Images/") + image.Guid + image.Extension);
                    }
                    db.Images.RemoveRange(brand.Images);
                    db.Brands.Remove(brand);
                    db.SaveChanges();
                }
                else
                {
                    throw new Exception("Brand not found for delete");
                }
            }
        }

        //[System.Web.Http.HttpPost]
        //public List<Store> GetStores()
        //{
        //    var brands = new List<Store>();
        //    using (var db = new DB())
        //    {
        //        brands.AddRange(db.Stores.Where(z=>z.).ToList().Select(item => new Store() { Id = item.Id, Name = item.Name, Description = item.Description, ImageId = item.Images.First().Id, File = item.Images.First().Guid + item.Images.First().Extension }));
        //        return brands;
        //    }
        //}

        [System.Web.Http.HttpPost]
        public List<Brand> GetBrands()
        {
            var brands = new List<Brand>();
            using (var db = new DB())
            {
                brands.AddRange(db.Brands.ToList().Select(item => new Brand() { Id = item.Id, Name = item.Name, Description = item.Description, ImageId = item.Images.First().Id, File = item.Images.First().Guid + item.Images.First().Extension }));
                return brands;
            }
        }
        [System.Web.Http.HttpPost]
        public int? InsertBrand(Brand brand)
        {
            using (var db = new DB())
            {
                if (brand == null)
                    throw new Exception("Brand NULL exception");
                var br = new Brands
                {
                    Name = brand.Name,
                    Description = brand.Description
                };
                db.Brands.Add(br);
                db.SaveChanges();
                db.Images.First(z => z.Id == brand.ImageId).BrandId = br.Id;
                db.SaveChanges();
                return br.Id;
            }
        }

        [System.Web.Http.HttpPost]
        public async Task<dynamic> UploadFile()
        {
            var GUID = new Guid();
            var extension = "";
            var ret = 0;
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            if (!provider.Contents.Any())
                throw new Exception("File not exists");
            foreach (var file in provider.Contents)
            {
                GUID = Guid.NewGuid();
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                extension = Path.GetExtension(filename);
                var bytes = await file.ReadAsByteArrayAsync();
                File.WriteAllBytes(System.Web.Hosting.HostingEnvironment.MapPath("~/Images/") + GUID + extension, bytes);
                using (var db = new DB())
                {
                    var img = new Images() { Guid = GUID, Extension = extension };
                    db.Images.Add(img);
                    db.SaveChanges();
                    ret = img.Id;
                }
            }
            return Json(new { ID = ret, file = string.Format("{0}{1}", GUID, extension) });
        }


        [System.Web.Http.HttpPost]
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

        [System.Web.Http.HttpPost]
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
                var Emails = model.Users.Where(z => z.Email.Trim().ToLower() != "sanjars@live.com").Select(u => u.Email).ToList();
                var users = manager.Users.Where(z => !Emails.Contains(z.Email)).ToList();
                foreach (var applicationUser in users)
                {
                    var f = manager.Delete(applicationUser);
                    if (!f.Succeeded)
                        throw new Exception(string.Join(", ", f.Errors));
                }
                foreach (var item in model.Users.Where(z => z.Email.ToLower().Trim() != "sanjars@live.com"))
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
