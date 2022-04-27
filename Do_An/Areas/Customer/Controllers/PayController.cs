using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Do_An.Areas.Customer.Models;
using Do_An.Areas.Customer.helper;
using Do_An.Frameworks;
using Do_An.Areas.Admin.Models;
using Do_An.Code;

namespace Do_An.Areas.Customer.Controllers
{
    public class PayController : Controller
    {
        private PayPal.Api.Payment payment;
        Connection cn = new Connection();
        THANHTOAN donhang = new THANHTOAN();
        // GET: Pay
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentWithPaypal(string sdt, string diachi, string chuthich)
        {

            //getting the apiContext  
            APIContext apiContext = Configuration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Pay/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, sdt);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return RedirectToAction("Index", "ThanhToan");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "ThanhToan");
            }
            //on successful payment, show success page to user.  
            donhang.Mua_Update(Configuration.SDT, Configuration.DiaChi, Configuration.tong, "Đã thanh toán", Configuration.ChuThich);
            string maDH = Configuration.MaDH;

            CustomerModel cus = new CustomerModel();
            OrderModel ord = new OrderModel();
            ViewBag.infoCustomer = cus.infoCustomer(Configuration.SDT); //Lấy thông tin khách hàng

            string noidung = "";
            ViewBag.infoOrder = ord.infoDonHang(maDH);
            foreach (var info in ViewBag.infoOrder)
            {
                noidung = noidung + "<tr>" + "<td>" + info.TenSP + "</td>" + "<td style=\"text-align:center;\">" + info.Gia.ToString("N0") + "</td>" + "<td style=\"text-align:center;\">" + info.SoLuong + "</td>" + "</tr>";
            }
            string notice = "<p style=\"color: red;\">" + " * Đơn hàng sẽ giao tới trong vòng 3 - 4 ngày (Không tính T7, CN)" + "</p>";
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Template/NewOrder.html"));

            var toEmail = "";
            foreach (var item in ViewBag.infoCustomer)
            {
                content = content.Replace("{{CustomerName}}", item.TenKH);
                content = content.Replace("{{Address}}", ord.addressOrderbySDT(Configuration.SDT));
                content = content.Replace("{{IDOrder}}", maDH + " <i>(Đã thanh toán)</i>");
                content = content.Replace("{{Order}}", noidung);
                content = content.Replace("{{Total}}", Configuration.tong.ToString("N0"));
                content = content.Replace("{{Notice}}", notice);
                toEmail = item.Email;
            }

            new MailHelper().SendMail(toEmail, "⌚ Đặt thành công đơn hàng [" + maDH + "] NOLOGO Shop", content);
            TempData["Alert-Message"] = "Đặt hàng thành công";
            return RedirectToAction("Index", "Home",new { area = "Customer"});
        }
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {

            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl, string sdt)
        {

            var sanpham = cn.XemGioHangs.SqlQuery("XemGioHang @SDT", new System.Data.SqlClient.SqlParameter("@SDT", sdt)).ToList();
            //thanh toán
            int tt = 0;
            foreach (var i in sanpham)
            {
                tt += (Convert.ToInt32(i.Gia) / 23000) * Convert.ToInt32(i.SoLuong);
            }
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            foreach (var sp in sanpham)
            {
                itemList.items.Add(new Item()
                {
                    name = sp.TenSP,
                    currency = "USD",
                    price = ((int)sp.Gia / 23000).ToString(),
                    quantity = "" + sp.SoLuong.ToString(),
                    //sku = "sku"
                });
            }
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "1",
                shipping = "2",
                subtotal = "" + tt
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "De-Montre",
                invoice_number = Convert.ToString((new Random()).Next(100000)), //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return payment.Create(apiContext);
        }
    }
}