using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Do_An.Areas.Customer.Models;
using Do_An.Frameworks;
using Newtonsoft.Json;

namespace Do_An.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        private MainDbContext db = new MainDbContext();
        CartModel gh = new CartModel();


        // GET: GIOHANGs
        public ActionResult Index(string sdt )
        {
           
            TempData["Selected"] = "Product";
            if (Session["Customer"] == null )
            {
                UrlPreviour.Value = Request.Url.ToString();
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            else
            {
                var sanpham = (from s in db.GIOHANGs where s.SDT == sdt select s).ToList();
                ViewBag.GioHang = sanpham;
            }
            return View();

        }

        [HttpPost]
        public ActionResult ThemGioHang(string MaSP, int soluong)
        {          
                if (Session["Customer"] != null )
                {
                    gh.ThemGioHang(Session["Phone"].ToString(), MaSP, soluong);                    
                }
            return View();
        }  
        public ViewResult MuaNgay(string masp,string sdt, int soluong)
        {
            if (Session["Customer"] != null)
            {
                gh.ThemGioHang(Session["Phone"].ToString(), masp, soluong);
            }
            return View("Index", "ThanhToan", new { sdt = sdt });
        }
        public ActionResult Logout()
        {
            Session["Customer"] = null;
            Session["Account"] = null;
            Session["Phone"] = null;
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
        public ActionResult XoaKhoiGio(string sdt, string MaSP)
        {
            if (Session["Customer"] != null)
            {
                gh.XoaSanPham(sdt, MaSP);
            }
            return RedirectToAction("Index","Cart", new { sdt = sdt});
        }
        public ActionResult QuayVe()
        {
            if(UrlPreviour.Value != "https://localhost:44380/Customer/Cart")
                return Redirect(UrlPreviour.Value);
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
        public ActionResult ChonTatCa()
        {
            if (Session["Customer"].ToString() != null || Session["Account"].ToString() != "admin")
            {
                bool check = true;
                ViewBag.CheckAll = check;
            }
            return RedirectToAction("Index", new { sdt = Session["Phone"].ToString(), check = true });
        }
        [HttpPost]
        public ActionResult SuaSoLuong(int STT, int SoLuong)
        {
            if (Session["Customer"] != null || Session["Account"].ToString() != "admin")
            {
                gh.SuaSoLuong(STT, SoLuong);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult CheckQuantity(int STT)
        {
            int SL = 0;
            if (Session["Customer"] != null || Session["Account"].ToString() != "admin")
            {
                SL = gh.getSL(STT);
            }
            var json = JsonConvert.SerializeObject(SL);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}