using Do_An.Areas.Admin.Models;
using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Do_An.Areas.Customer.Controllers
{
    public class InfoCustomerController : Controller
    {
        // GET: Customer/InfoCustomer
        public ActionResult Index()
        {
            if (Session["Customer"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                if (Session["URL"] == null)
                {
                    Session["URL"] = HttpContext.Request.UrlReferrer.AbsoluteUri.ToString();
                    ViewBag.URL = Session["URL"];
                }
                else
                {
                    ViewBag.URL = Session["URL"];
                }
                CustomerModel cus = new CustomerModel();
                ViewBag.Customer = cus.infoCustomer(Session["Phone"].ToString());
                return View();
            }
           
        }
        [HttpPost]
        public ActionResult Index(INFORMATION entity)
        {
            if (Session["Customer"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {

                CustomerModel cus = new CustomerModel();
                cus.changeInfo(entity);
                TempData["Alert-Message"] = "Thay đổi thông tin thành công";

                return Redirect(Request.UrlReferrer.ToString());
            }
        }
    }
}