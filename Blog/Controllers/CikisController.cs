using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class CikisController : Controller
    {
        public ActionResult Index()
        {
            Session["giris"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}