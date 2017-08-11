using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Discounts.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Discounts.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : GlobalController
    {      
        public ActionResult Index()
        {
            return View();
        }
    }
}
