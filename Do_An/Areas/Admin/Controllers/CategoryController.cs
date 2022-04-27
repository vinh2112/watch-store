using Do_An.Areas.Admin.Models;
using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Do_An.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        public ActionResult Index()
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                Session["URL"] = null;
                var category = new CategoryModel();
                var model = category.ListAllCategory();
                ViewBag.Category = model;
                return View();
            }
        }
        public ActionResult Edit(string maDM, string type = null)
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
                    TempData["Alert-Message"] = "Chỉnh sửa danh mục thành công";
                    TempData["AlertType"] = "alert-success";
                }
                else
                {
                    if (type == "fail")
                    {
                        TempData["Alert-Message"] = "Chỉnh sửa danh mục thất bại";
                        TempData["AlertType"] = "alert-danger";
                    }
                }
                var category = new CategoryModel();
                ViewBag.Category = category.infoCategory(maDM);
                return View();
            }
        }
        [HttpPost]
        public ActionResult Edit(DANHMUC category)
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                if (ModelState.IsValid)
                {

                    var th = new CategoryModel();
                    bool res = th.updateCategory(category);
                    if (res)
                    {
                        return RedirectToAction("Edit", "Category", new { maDM = category.MaDM, type = "success" });
                    }
                }
                return View(category);
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
                    TempData["Alert-Message"] = "Thêm danh mục thành công";
                    TempData["AlertType"] = "alert-success";
                }
                else
                {
                    if (type == "fail")
                    {
                        TempData["Alert-Message"] = "Thêm danh mục thất bại";
                        TempData["AlertType"] = "alert-danger";
                    }
                }
                return View();
            }
        }
        [HttpPost]
        public ActionResult Insert(DANHMUC entity)
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                if (ModelState.IsValid)
                {

                    var dm = new CategoryModel();
                    bool res = dm.insertCategory(entity);
                    if (res)
                    {
                        return RedirectToAction("Insert", "Category", new { type = "success" });
                    }
                    else
                    {
                        return RedirectToAction("Insert", "Category", new { type = "fail" });
                    }
                }
                return View(entity);
            }
        }
    }
}