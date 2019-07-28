using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Blog.Filters
{
    public class CustomLoginFilter : ActionFilterAttribute
    {
        private Blog.DAL.BlogEntities db = new DAL.BlogEntities();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var id = (Blog.DAL.Uye)filterContext.HttpContext.Session["giris"];

            if (id != null)
            {
                var UyeID = db.Uye.Find(id.ID);

                if (UyeID.Yetki == 1)
                {
                    //base.OnActionExecuting(filterContext);
                    return;
                }

            }

            filterContext.Result = new RedirectToRouteResult(
                                   new RouteValueDictionary
                                   {
                                       { "action", "Giris" },
                                       { "controller", "Home" }
                                   }
                                );
        }

      

    }
}