using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ActiveBlendedStudy.Filters
{
    public class SessionAuthorizeAttribute : AuthorizeAttribute,IAuthorizationFilter
    {
        public string role { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if(httpContext.Session != null && httpContext.Session["User_ID"] != null && httpContext.Session["Role"] != null && httpContext.Session["Schedule_ID"] != null && httpContext.Session["Course_ID"] != null)
            {
                if(string.IsNullOrEmpty(role))
                {
                    return true;
                }
                else if(httpContext.Session["Role"].ToString().ToUpper().Equals(role.ToUpper()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new
               RouteValueDictionary(new { controller = "Home" , action = "Unauthorised" }));
            //filterContext.Result = new RedirectResult("/some/error");
        }
    }
}