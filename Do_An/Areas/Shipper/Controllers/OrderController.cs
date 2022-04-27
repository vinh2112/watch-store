using Do_An.Areas.Shipper.Models;
using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Do_An.Areas.Shipper.Controllers
{
    public class OrderController : Controller
    {
        // GET: Shipper/Order
        public ActionResult Index()
        {
            if (Session["Shipper"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                var order = new OrderModel();
                ViewBag.Order = order.ListAllOrder();
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
        public ActionResult Shipping(string maDH, string SDT)
        {
            if (Session["Shipper"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                var order = new OrderModel();
                ViewBag.infoOrder = order.infoDonHang(maDH);
                try
                {
                    order.Update("Shipping", maDH, SDT);
                    TempData["Alert-Message"] = "Xác nhận đơn hàng " + maDH + " thành công";
                    TempData["AlertType"] = "alert-success";
                }
                catch { }
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
        public ActionResult Receive(string sdt)
        {
            if (Session["Shipper"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            else
            {
                var order = new OrderModel();
                ViewBag.Order = order.ListAllShipping(sdt);
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
        public ActionResult XacNhan(string maDH, string SDT)
        {
            if (Session["Shipper"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                var order = new OrderModel();
                ViewBag.infoOrder = order.infoDonHang(maDH);
                try
                {
                    order.Update("Delivered", maDH, SDT);
                    TempData["Alert-Message"] = "Xác nhận giao hàng " + maDH + " thành công";
                    TempData["AlertType"] = "alert-success";
                }
                catch { }
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
        public ActionResult KhongNhan(string maDH, string SDT)
        {
            if (Session["Shipper"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            else
            {
                var order = new OrderModel();
                ViewBag.infoOrder = order.infoDonHang(maDH);
                try
                {
                    order.Update("Cancel",maDH, SDT);
                    TempData["Alert-Message"] = "Xác nhận đơn hàng " + maDH + " thành công";
                    TempData["AlertType"] = "alert-success";
                }
                catch { }
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
        public ActionResult Huy(string maDH, string SDT)
        {
            if (Session["Shipper"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            else
            {
                var order = new OrderModel();
                ViewBag.infoOrder = order.infoDonHang(maDH);
                try
                {
                    order.Update("Destroy", maDH, SDT);
                    TempData["Alert-Message"] = "Xác nhận hủy đơn hàng " + maDH + " thành công";
                    TempData["AlertType"] = "alert-success";
                }
                catch { }
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
        public ActionResult Delivered(string sdt)
        {
            if (Session["Shipper"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            else
            {
                var order = new OrderModel();
                ViewBag.Order = order.ListAllDelivered(sdt);
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