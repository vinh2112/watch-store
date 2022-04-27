using Do_An.Areas.Shipper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Do_An.Areas.Shipper.Controllers
{
    public class HomeController : Controller
    {
        // GET: Shipper/Home
        public ActionResult Index()
        {
            if (Session["Shipper"] != null)
            {
                ThongKeModel tk = new ThongKeModel();
                ViewBag.ThongKe = tk.GetThongKe(Session["UserN"].ToString());
                List<string> lstString = new List<string>();
                List<int> doanhthu = new List<int>();
                foreach (var item in ViewBag.ThongKe)
                {
                    DateTime date = item.NgayXacNhan;
                    lstString.Add(date.Day.ToString() + "/" + date.Month.ToString());
                    doanhthu.Add(item.ThongKe);
                }
                ViewBag.Date = lstString;
                ViewBag.DoanhThu = doanhthu;
                ViewData["DonHT"] = tk.TotalAccept(Session["UserN"].ToString());
                ViewData["DonHuy"] = tk.TotalCancel(Session["UserN"].ToString());
                ViewData["DonNhan"] = tk.Total(Session["UserN"].ToString());
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
        }
        public ActionResult Logout()
        {
            Session["Shipper"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }


    }
}