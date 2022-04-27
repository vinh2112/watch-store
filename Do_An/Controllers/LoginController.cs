using Do_An.Code;
using Do_An.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Do_An.Frameworks;
using Do_An.Areas.Admin.Models;

namespace Do_An.Controllers
{
    public class LoginController : Controller
    {      
        MainDbContext db = new MainDbContext();
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {          
            if(Session["Account"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else if(Session["Customer"] != null)
            {

                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(USER model)
        {
            Session["Account"] = null;
            int check = db.USERS.Where(x => x.UserN == model.UserN && x.PassW == model.PassW && x.Roles == 2).Count();
            //var result = new AccountModel().Login(model.Username, model.Password);
            if (Membership.ValidateUser(model.UserN, model.PassW) && ModelState.IsValid && model.UserN == "admin")
            {
                //SessionHelper.SetSession(new UserSession() { UserName = model.Username });
                FormsAuthentication.SetAuthCookie(model.UserN, true);
                Session["Account"] = model.UserN;
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else if (Membership.ValidateUser(model.UserN, model.PassW) && ModelState.IsValid && check != 0)
            {
                FormsAuthentication.SetAuthCookie(model.UserN, true);
                Session["Shipper"] = db.INFORMATION.Where(x => x.SDT == model.UserN).Select(x => x.TenKH).FirstOrDefault();
                Session["UserN"] = model.UserN;
                return RedirectToAction("Index", "Home", new { area = "Shipper" });
            }
            else
            {
                Session["Customer"] = null;
                if (Membership.ValidateUser(model.UserN, model.PassW) && ModelState.IsValid)
                {
                    Session["Customer"] = db.INFORMATION.Where(x => x.SDT == model.UserN).Select(x => x.TenKH).FirstOrDefault();
                    Session["Phone"] = model.UserN;
                    Session["Account"] = null;
                    //SessionHelper.SetSession(new UserSession() { UserName = model.Username });
                    if (UrlPreviour.Value == null)
                    {
                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }
                    else
                        return Redirect(UrlPreviour.Value);
                        //return RedirectToAction("Index", "Product_Infor", new { area = "Customer", Masp = Session["masp"] });

                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng !!");
                }

            }
            return View(model);
        }
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(INFORMATION entity)
        {
            MainDbContext db = new MainDbContext();
            int count = db.INFORMATION.Where(x => x.SDT == entity.SDT).Count();
            if(count > 0)
            {
                try
                {
                    CustomerModel cus = new CustomerModel();
                    ViewBag.infoCustomer = cus.infoCustomer(entity.SDT);

                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Template/OTP.html"));

                    Random rd = new Random();
                    string rdNumber = (rd.Next(100000, 999999)).ToString();
                    Session["OTP"] = rdNumber;

                    var toEmail = "";
                    foreach (var item in ViewBag.infoCustomer)
                    {
                        content = content.Replace("{{OTP}}", Session["OTP"].ToString());
                        toEmail = item.Email;
                    }

                    new MailHelper().SendMail(toEmail, "Lấy lại mật khẩu", content);
                    Session["PhoneNumber"] = entity.SDT;
                    return RedirectToAction("Verify", "Login");
                }
                catch { }
            }
            else
            {
                TempData["Error"] = "Số điện thoại không tồn tại";
            }
            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Verify()
        {
            if (Session["OTP"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("ForgetPassword", "Login");
            }
        }
        [HttpPost]
        public ActionResult Verify(string OTP)
        {
            if(Session["OTP"] != null)
            {
                if (OTP == Session["OTP"].ToString())
                {
                    Session["OTPStatus"] = "success";
                    return RedirectToAction("Password","Login");
                }
                else
                {
                    TempData["Error"] = "Mã OTP không đúng";
                }
            }

            return Redirect(Request.UrlReferrer.ToString());
        }
        public ActionResult Password()
        {
            if(Session["OTP"] != null)
            {
                if(Session["OTPStatus"].ToString() == "success")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Verify", "Login");
                }
            }
            else
            {
                return RedirectToAction("ForgetPassword", "Login");
            }
        }
        [HttpPost]
        public ActionResult Password(string Pass, string RePass)
        {
            if(Pass == RePass)
            {
                AccountModel acc = new AccountModel();
                acc.ResetPassword(Session["PhoneNumber"].ToString(), Pass);
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["Error"] = "Xác nhận mật khẩu không đúng";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["Account"] = null;
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
    }
}