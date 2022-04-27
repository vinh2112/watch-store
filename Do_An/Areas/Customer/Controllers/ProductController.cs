using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Do_An.Frameworks;
using PagedList;

namespace Do_An.Areas.Customer.Controllers
{
    public class ProductController : Controller
    {
        // GET: Customer/Product
        MainDbContext db = new MainDbContext();
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            TempData["Selected"] = "Product";
            var sanpham = (from s in db.SANPHAMs where s.TinhTrang == "Đang bán" orderby s.Gia ascending select s).ToList().ToPagedList(page,pageSize);
            return View(sanpham);
        }
        public ActionResult GiaThapNhat(int page = 1, int pageSize = 15)
        {
            TempData["Selected"] = "Product";
            var sanpham = (from s in db.SANPHAMs orderby s.Gia descending where s.TinhTrang == "Đang bán" select s).ToList().ToPagedList(page, pageSize);
            return View(sanpham);
        }
        public ActionResult Sale(int page = 1, int pageSize = 15)
        {
            TempData["Selected"] = "Product";
            var sanpham = (from s in db.SANPHAMs where s.Discount != 0 && s.TinhTrang == "Đang bán" select s).ToList().ToPagedList(page, pageSize);
            return View(sanpham);
        }
        public ActionResult XemTheoDanhMuc(string MaDM, string sapxep="tangdan", int page = 1, int pageSize = 15)
        {
            TempData["Selected"] = "Category";
            var DM=db.DANHMUCs.Find(MaDM);
            ViewBag.DM = DM;
            ViewBag.Sort = sapxep;
            if (sapxep == null)
            {
                var sanpham = (from s in db.SANPHAMs where s.MaDM == MaDM && s.TinhTrang == "Đang bán" select s).ToList().ToPagedList(page, pageSize);
                return View(sanpham);
            }
            else if (sapxep == "tangdan")
            {
                var sanpham = (from s in db.SANPHAMs orderby s.Gia ascending where s.MaDM == MaDM && s.TinhTrang == "Đang bán" select s).ToList().ToPagedList(page, pageSize);
                return View(sanpham);
            }
            else if (sapxep == "giamdan")
            {
                var sanpham = (from s in db.SANPHAMs orderby s.Gia descending where s.MaDM == MaDM && s.TinhTrang == "Đang bán" select s).ToList().ToPagedList(page, pageSize);
                return View(sanpham);
            }
            else
            {
                var sanpham = (from s in db.SANPHAMs where s.Discount != 0 && s.MaDM== MaDM && s.TinhTrang == "Đang bán" select s).ToList().ToPagedList(page, pageSize);
                return View(sanpham);
            }
        }
        public ActionResult XemTheoThuongHieu(string MaTH, string sapxep= "tangdan", int page = 1, int pageSize = 15)
        {
            TempData["Selected"] = "Brand";
            var TH = db.BRANDs.Find(MaTH);
            ViewBag.TH = TH;
            ViewBag.Sort = sapxep;
            if (sapxep == null)
            {
                var sanpham = (from s in db.SANPHAMs where s.MaTH == MaTH && s.TinhTrang == "Đang bán" select s).ToList().ToPagedList(page, pageSize);
                return View(sanpham);
            }
            else if (sapxep == "tangdan")
            {
                var sanpham = (from s in db.SANPHAMs orderby s.Gia ascending where s.MaTH == MaTH && s.TinhTrang == "Đang bán" select s).ToList().ToPagedList(page, pageSize);
                return View(sanpham);
            }
            else if (sapxep=="giamdan")
            {
                var sanpham = (from s in db.SANPHAMs orderby s.Gia descending where s.MaTH == MaTH && s.TinhTrang == "Đang bán" select s).ToList().ToPagedList(page, pageSize);
                return View(sanpham);
            }
            else
            {
                var sanpham = (from s in db.SANPHAMs where s.Discount != 0 && s.MaTH== MaTH && s.TinhTrang == "Đang bán" select s).ToList().ToPagedList(page, pageSize);
                return View(sanpham);
            }
        }
        public ActionResult TimKiem(string timkiem,string sapxep = "tangdan", int page = 1, int pageSize = 15)
        {
            ViewBag.Keyword = timkiem;
            ViewBag.Sort = sapxep;
            TempData["Selected"] = "none";
            if (sapxep == null)
            {
                var sanpham = db.SANPHAMs.Where(c => c.TenSP.Contains(timkiem)).Where(x => x.TinhTrang == "Đang bán").ToList().ToPagedList(page, pageSize);
                return View(sanpham);
            }
            else if (sapxep == "tangdan")
            {
                var sanpham = db.SANPHAMs.Where(c => c.TenSP.Contains(timkiem)).Where(x => x.TinhTrang == "Đang bán").OrderBy(x => x.Gia).ToList().ToPagedList(page, pageSize);
                return View(sanpham);
            }
            else if (sapxep == "giamdan")
            {
                var sanpham = db.SANPHAMs.Where(c => c.TenSP.Contains(timkiem)).Where(x => x.TinhTrang == "Đang bán").OrderByDescending(x => x.Gia).ToList().ToPagedList(page, pageSize);
                return View(sanpham);
            }
            else
            {
                var sanpham = db.SANPHAMs.Where(c => c.TenSP.Contains(timkiem)).Where(x => x.Discount != 0 && x.TinhTrang == "Đang bán").ToList().ToPagedList(page, pageSize);
                return View(sanpham);
            }
        }
        [HttpPost]
        public JsonResult ListItem(string q)
        {
            List<string> sanpham = db.SANPHAMs.Where(x => x.TenSP.Contains(q)).Select(x=>x.TenSP).ToList();
            return Json(new
            {
                data = sanpham,
                status = true
            }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ViewTinTuc(string news=null)
        {
            TempData["Selected"] = "News";
            ViewBag.news = news;    
            return View();
        }
    }
}