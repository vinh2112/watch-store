using Do_An.Areas.Admin.Models;
using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Do_An.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        // GET: Admin/Product
        public ActionResult Index()
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                Session["URL"] = null;

                var product = new ProductModel();
                var brand = new BrandModel();
                var category = new CategoryModel();
                ViewBag.Product = product.ListAll();
                ViewBag.Brand = brand.ListAllBrand();
                ViewBag.Category = category.ListAllCategory();
                return View();
            }
        }

        public ActionResult SanPhamTheoThuongHieu(string maTH)
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                Session["URL"] = null;
                ViewBag.maTh = maTH;
                var product = new ProductModel();
                var brand = new BrandModel();
                var category = new CategoryModel();
                var model = product.ListAllbyBrand(maTH);
                ViewBag.Brand = brand.ListAllBrand();
                ViewBag.Category = category.ListAllCategory();
                return View(model);
            }
        }
        public ActionResult SanPhamTheoDanhMuc(string maDM)
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                Session["URL"] = null;

                ViewBag.maDM = maDM;
                var product = new ProductModel();
                var brand = new BrandModel();
                var category = new CategoryModel();
                var model = product.ListAllbyCategory(maDM);
                ViewBag.Brand = brand.ListAllBrand();
                ViewBag.Category = category.ListAllCategory();
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult Edit(string maSP, string type = null)
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
                if (type == "success")
                {
                    TempData["Alert-Message"] = "Chỉnh sửa sản phẩm " + maSP + " thành công";
                    TempData["AlertType"] = "alert-success";
                }
                else
                {
                    if (type == "fail")
                    {
                        TempData["Alert-Message"] = "Chỉnh sửa sản phẩm " + maSP + " thất bại";
                        TempData["AlertType"] = "alert-danger";
                    }
                }
                var product = new ProductModel();
                var brand = new BrandModel();
                var category = new CategoryModel();
                ViewBag.Product = product.infoProduct(maSP);
                ViewBag.Brand = brand.ListAllBrand();
                ViewBag.Category = category.ListAllCategory();
                return View();
            }
        }

        [HttpPost]
        public ActionResult Edit(SANPHAM product, HttpPostedFileBase file, string cates, string status)
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                if (ModelState.IsValid)
                {
                    product.MaDM = cates;
                    product.TinhTrang = status;
                    if (file != null && file.ContentLength > 0)
                    {
                        string filename = System.IO.Path.GetFileName(file.FileName);
                        string urlfile = Server.MapPath("~/Images/" + filename);
                        file.SaveAs(urlfile);

                        product.HinhAnh = "/Images/" + filename;
                    }

                    var pro = new ProductModel();
                    bool res = pro.updateProduct(product);
                    if (res)
                    {
                        return RedirectToAction("Edit", "Product", new { maSP = product.MaSP, type = "success" });
                    }
                }
                return View(product);
            }
        }
        [HttpGet]
        public ActionResult Delete(string maSP,string type)
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                var pro = new ProductModel();
                if (type == "Invisible")
                {
                    pro.deleteProduct(maSP, type);
                    TempData["Alert-Message"] = "Ẩn sản phẩm " + maSP + " thành công";
                    TempData["AlertType"] = "alert-success";
                }
                else
                {
                    pro.deleteProduct(maSP, type);
                    TempData["Alert-Message"] = "Hiện sản phẩm " + maSP + " thành công";
                    TempData["AlertType"] = "alert-success";
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
        public ActionResult Insert(string type = null)
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                //Lưu URL
                if (Session["URL"] == null)
                {
                    Session["URL"] = HttpContext.Request.UrlReferrer.AbsoluteUri.ToString();
                    ViewBag.URL = Session["URL"];
                }
                else
                {
                    ViewBag.URL = Session["URL"];
                }

                if (type == "success")
                {
                    TempData["Alert-Message"] = "Thêm sản phẩm thành công";
                    TempData["AlertType"] = "alert-success";
                }
                else
                {
                    if (type == "fail")
                    {
                        TempData["Alert-Message"] = "Thêm sản phẩm thất bại";
                        TempData["AlertType"] = "alert-danger";
                    }
                }
                var brand = new BrandModel();
                var category = new CategoryModel();
                ViewBag.Brand = brand.ListAllBrand();
                ViewBag.Category = category.ListAllCategory();
                return View();
            }
        }
        [HttpPost]
        public ActionResult Insert(SANPHAM product, string brand, string category, HttpPostedFileBase file)
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                product.MaTH = brand;
                product.MaDM = category;

                if (file != null && file.ContentLength > 0)
                {
                    string filename = System.IO.Path.GetFileName(file.FileName);
                    string urlfile = Server.MapPath("~/Images/" + filename);
                    file.SaveAs(urlfile);

                    product.HinhAnh = "/Images/" + filename;
                }

                var pro = new ProductModel();
                bool res = pro.insertProduct(product);
                if (res)
                {
                    return RedirectToAction("Insert", "Product", new { type = "success" });
                }
                else
                {
                    return RedirectToAction("Insert", new { type = "fail" });
                }
            }
        }

        public ActionResult Stored()
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                Session["URL"] = null;

                var product = new ProductModel();
                var brand = new BrandModel();
                var category = new CategoryModel();
                var model = product.ListAlllowQuantity();
                ViewBag.Brand = brand.ListAllBrand();
                ViewBag.Category = category.ListAllCategory();
                return View(model);
            }
        }
    }
}