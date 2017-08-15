using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Discounts.Controllers
{
    public abstract class GlobalController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.loggedIn = User.Identity.IsAuthenticated;
            ViewBag.userId = User.Identity.GetUserId();
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            ViewBag.roles = identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            base.OnActionExecuting(filterContext);
        }
    }
}