using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Do_An.Areas.Customer.Controllers
{
    [Authorize]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        // GET: Customer/Home
        public ActionResult Index()
        {
            TempData["Selected"] = "Main";
            return View();
        }
    }
}