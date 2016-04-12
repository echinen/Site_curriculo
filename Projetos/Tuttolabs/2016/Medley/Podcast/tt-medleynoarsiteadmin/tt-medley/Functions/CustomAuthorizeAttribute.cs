using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using tt_medley.Models;

namespace tt_medley.Functions
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (SessionManager.CheckSession("userId") == true)
                return true;
            else
                return false;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (SessionManager.CheckSession("userId") == false)
            {
                filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary
                        {
                            { "action", "Login" },
                            { "controller", "Account" }
                        });
            }
            else
                base.HandleUnauthorizedRequest(filterContext);
            //filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}