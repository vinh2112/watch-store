using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Do_An.Frameworks;
using Do_An.Areas.Customer.Models;

namespace Do_An.Areas.Customer.Controllers
{
    public class Product_InforController : Controller
    {
        // GET: Customer/Product_Infor
        private MainDbContext db = new MainDbContext();
        public ActionResult Index(string Masp)
        {
                Session["masp"] = Masp;
                TempData["Selected"] = "Product";
                var lst = db.SANPHAMs.Where(x => x.MaSP == Masp).OrderBy(x => x.MaSP).SingleOrDefault();
                ViewBag.SANPHAM = lst;
                ViewBag.Brand = db.BRANDs.Find(ViewBag.SANPHAM.MaTH.ToString()).TenTH;
                var lis = new Recommand();
                ViewBag.SameSanPham = lis.RECOMMAND(Masp);
                return View(lst);
            
        }
        [HttpGet]
        public ActionResult test ()
        {
            return  RedirectToAction("Index", "Login", new { area = "" });
        }
    }
}