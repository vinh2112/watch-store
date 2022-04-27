using Do_An.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Do_An.Areas.Admin.Controllers
{
    public class DoanhThuController : Controller
    {
        // GET: Admin/DoanhThu
        public ActionResult Index()
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {

                var doanhthu = new DoanhThuModel();
                List<int> doanhthuthang = new List<int>();
                for (int i = 0; i < 12; i++)
                {
                    doanhthuthang.Add(doanhthu.DoanhThuThang(i + 1));
                }
                ViewBag.DoanhThu = doanhthuthang.ToList();

                return View();
            }
        }
    }
}