using Do_An.Areas.Admin.Models;
using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Do_An.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        // GET: Admin/Brand
        public ActionResult Index()
        {
            if (Session["Account"] == null)
            {               
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                Session["URL"] = null;
                var brand = new BrandModel();
                var model = brand.ListAllBrand();
                ViewBag.Brand = model;
                return View();
            }
            
        }
        public ActionResult Edit(string maTH, string type = null)
        {

            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {

                if (Session["Account"].ToString() != null)
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
                        TempData["Alert-Message"] = "Chỉnh sửa sản phẩm thành công";
                        TempData["AlertType"] = "alert-success";
                    }
                    else
                    {
                        if (type == "fail")
                        {
                            TempData["Alert-Message"] = "Chỉnh sửa sản phẩm thất bại";
                            TempData["AlertType"] = "alert-danger";
                        }
                    }
                    var brand = new BrandModel();
                    ViewBag.Brand = brand.infoBrand(maTH);
                }
                return View();
            }

        }
        [HttpPost]
        public ActionResult Edit(BRAND brand, HttpPostedFileBase file)
        {

            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string filename = System.IO.Path.GetFileName(file.FileName);
                        string urlfile = Server.MapPath("~/Images/" + filename);
                        file.SaveAs(urlfile);

                        brand.HinhAnh = "/Images/" + filename;
                    }

                    var th = new BrandModel();
                    bool res = th.updateBrand(brand);
                    if (res)
                    {
                        return RedirectToAction("Edit", "Brand", new { maTH = brand.MaTH, type = "success" });
                    }
                }

                return View(brand);
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

                return View();
            }
        }
        [HttpPost]
        public ActionResult Insert(BRAND entity, HttpPostedFileBase file)
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {

                if (ModelState.IsValid)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string filename = System.IO.Path.GetFileName(file.FileName);
                        string urlfile = Server.MapPath("~/Images/" + filename);
                        file.SaveAs(urlfile);

                        entity.HinhAnh = "/Images/" + filename;
                    }

                    var th = new BrandModel();
                    bool res = th.insertBrand(entity);
                    if (res)
                    {
                        return RedirectToAction("Insert", "Brand", new { type = "success" });
                    }
                    else
                    {
                        return RedirectToAction("Insert", "Brand", new { type = "fail" });
                    }
                }

                return View(entity);
            }
        }
    }
}