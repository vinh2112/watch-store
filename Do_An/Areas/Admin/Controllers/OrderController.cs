using Do_An.Areas.Admin.Models;
using Do_An.Code;
using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Do_An.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        // GET: Admin/Order
        public ActionResult Index()
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                var order = new OrderModel();
                ViewBag.Order = order.ListAllSuccessOrder();
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
        public ActionResult Waiting()
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                var order = new OrderModel();
                ViewBag.Order = order.ListAllWaitingOrder();
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
        public ActionResult Shipping()
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                var order = new OrderModel();
                ViewBag.Order = order.ListAllShippingOrder();
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
        public ActionResult Canceled()
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                var order = new OrderModel();
                ViewBag.Order = order.ListAllCanceledOrder();
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
        public ActionResult ShipOrder(string maDH)
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                var order = new OrderModel();
                ViewBag.infoOrder = order.infoDonHang(maDH);
                try
                {
                    order.updateOrder(maDH,"Đang giao");
                    TempData["Alert-Message"] = "Xác nhận đơn hàng " + maDH + " thành công";
                    TempData["AlertType"] = "alert-success";
                }
                catch
                {
                    TempData["Alert-Message"] = "Hủy đơn hàng " + maDH + " thất bại";
                    TempData["AlertType"] = "alert-success";
                }

                return Redirect(Request.UrlReferrer.ToString());
            }
        }
        public ActionResult CancelOrder(string maDH)
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                var order = new OrderModel();
                ViewBag.infoOrder = order.infoDonHang(maDH);
                try
                {
                    foreach (var item in ViewBag.infoOrder)
                    {
                        order.editQuantity(item.MaSP, item.SoLuong);
                    }
                    order.updateOrder(maDH,"Đã hủy");
                    TempData["Alert-Message"] = "Hủy đơn hàng " + maDH + " thành công";
                    TempData["AlertType"] = "alert-success";
                }
                catch
                {
                    TempData["Alert-Message"] = "Hủy đơn hàng " + maDH + " thất bại";
                    TempData["AlertType"] = "alert-success";
                }

                return Redirect(Request.UrlReferrer.ToString());
            }
        }
        public ActionResult VerifyOrder(string maDH, string SDT)
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {
                int TongTien = 0;
                var order = new OrderModel();
                ViewBag.infoOrder = order.infoDonHang(maDH);
                foreach (var item in ViewBag.infoOrder)
                {
                    TongTien += item.Gia * item.SoLuong;
                }
                TempData["Alert-Message"] = "Xác nhận đơn hàng " + maDH + " thành công";
                TempData["AlertType"] = "alert-success";

                try
                {
                    CustomerModel cus = new CustomerModel();
                    ViewBag.infoCustomer = cus.infoCustomer(SDT); //Lấy thông tin khách hàng

                    string noidung = "";
                    ViewBag.infoOrder = order.infoDonHang(maDH);
                    foreach (var info in ViewBag.infoOrder)
                    {
                        noidung = noidung + "<tr>" + "<td>" + info.TenSP + "</td>" + "<td style=\"text-align:center;\">" + info.Gia.ToString("N0") + "</td>" + "<td style=\"text-align:center;\">" + info.SoLuong + "</td>" + "</tr>";
                    }

                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Template/NewOrder.html"));

                    var toEmail = "";
                    foreach (var item in ViewBag.infoCustomer)
                    {
                        content = content.Replace("{{CustomerName}}", item.TenKH);
                        content = content.Replace("{{Address}}", order.addressOrderbySDT(SDT));
                        content = content.Replace("{{IDOrder}}", maDH + " <i>(Giao thành công)</i>");
                        content = content.Replace("{{Order}}", noidung);
                        content = content.Replace("{{Total}}", TongTien.ToString("N0"));
                        content = content.Replace("{{Notice}}", "");
                        toEmail = item.Email;
                    }

                    new MailHelper().SendMail(toEmail, "⌚ Đơn hàng [" + maDH + "] NOLOGO Shop", content);
                    order.updateOrder(maDH, "Đã giao");
                }
                catch { }

                return Redirect(Request.UrlReferrer.ToString());
            }
        }
        public ActionResult ChangeInfo(string maDH,string DiaChi)
        {
            if (Session["Account"] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "" });

            }
            else
            {

                var order = new OrderModel();
                order.chageInfo(maDH, DiaChi);
                TempData["Alert-Message"] = "Thay đổi thông tin đơn hàng " + maDH + " thành công";
                TempData["AlertType"] = "alert-success";

                return Redirect(Request.UrlReferrer.ToString());
            }
        }

    }
}