using Do_An.Areas.Admin.Models;
using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Do_An.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Admin/Customer
        public ActionResult Index()
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                Session["URL"] = null;
                var customer = new CustomerModel();
                var model = customer.ListAllCustomer();
                ViewBag.Customer = model;
                return View();
            }
        }
        public ActionResult Info(string SDT)
        {
            if (Session["Account"] == null)
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

                var customer = new CustomerModel();
                var order = new OrderModel();
                ViewBag.Customer = customer.infoCustomer(SDT);
                ViewBag.Order = order.ListOrderbySDT(SDT);
                foreach (var item in ViewBag.Order)
                {
                    List<string> lstString = new List<string>();
                    ViewBag.infoOrder = order.infoDonHang(item.MaDH);
                    foreach (var info in ViewBag.infoOrder)
                    {
                        lstString.Add(info.TenSP + "   [ -" + info.SoLuong + "- ]  ");
                    }
                    lstString.TrimExcess();
                    ViewData[item.MaDH] = lstString;
                }

                return View();
            }
        }
    }
}