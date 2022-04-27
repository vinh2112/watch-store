using Do_An.Areas.Admin.Models;
using Do_An.Frameworks;
using Do_An.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Do_An.Areas.Admin.Controllers
{
    public class ShipperController : Controller
    {
        // GET: Admin/Shipper
        public ActionResult Index()
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                Session["URL"] = null;
                ShipperModel shipper = new ShipperModel();
                ViewBag.Shipper = shipper.getShipper();
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

                ShipperModel shipper = new ShipperModel();
                ViewBag.Shipper = shipper.getInfoShipper(SDT);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Info(SHIPPER entity)
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                ShipperModel shipper = new ShipperModel();
                if (shipper.updateShipepr(entity))
                {
                    TempData["Alert-Message"] = "Chỉnh sửa thông tin Shipper thành công";
                    TempData["AlertType"] = "alert-success";
                    return RedirectToAction("Info", new { SDT = entity.SDT });
                }
                else
                {
                    TempData["Alert-Message"] = "Chỉnh sửa thông tin Shipper thất bại";
                    TempData["AlertType"] = "alert-danger";
                }
                return View(entity);
            }
        }

        public ActionResult Insert()
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
                return View();
            }
        }

        [HttpPost]
        public ActionResult Insert(SHIPPER entity, string PassW, string rePassW)
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
                if (!new AccountModel().CheckExist(entity.SDT))
                {
                    if(PassW == rePassW)
                    {
                        ShipperModel shipper = new ShipperModel();
                        if(shipper.insertShipper(entity, PassW))
                        {
                            TempData["Alert-Message"] = "Thêm Shipper thành công";
                            TempData["AlertType"] = "alert-success";
                        }
                        else
                        {
                            TempData["Alert-Message"] = "Thêm Shipper thất bại";
                            TempData["AlertType"] = "alert-danger";
                        }
                    }
                    else
                    {
                        TempData["Alert-Message"] = "Xác thực mật khẩu không đúng";
                        TempData["AlertType"] = "alert-danger";
                    }
                }
                else
                {
                    TempData["Alert-Message"] = "Số điện thoại đã tồn tại";
                    TempData["AlertType"] = "alert-danger";
                }
                return View(entity);
            }
        }
    }
}