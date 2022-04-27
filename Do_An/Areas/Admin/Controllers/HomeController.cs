using Do_An.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Do_An.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            if(Session["Account"] == null)
            {             
                return RedirectToAction("Index","Login", new { area = ""});
            }
            else
            {
                CustomerModel cus = new CustomerModel();
                ProductModel pro = new ProductModel();
                OrderModel ord = new OrderModel();
                DoanhThuModel doanhthu = new DoanhThuModel();
                string customer = cus.countCustomer();
                string product = pro.countProduct();
                string order = ord.countOrder();
                string dt = doanhthu.DoanhThu();
                List<int> doanhthuthang = new List<int>();
                for (int i = 0; i < 12; i++)
                {
                    doanhthuthang.Add(doanhthu.DoanhThuThang(i + 1));
                }
                ViewBag.DoanhThuThang = doanhthuthang.ToList();
                ViewBag.countCus = customer;
                ViewBag.countPro = product;
                ViewBag.countOrder = order;
                ViewBag.DoanhThu = dt;
                return View();
            }
            

        }
    }
}