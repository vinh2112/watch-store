using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Do_An.Areas.Customer.Models;
using Do_An.Areas.Customer.helper;
using Do_An.Areas.Admin.Models;
using Do_An.Code;

namespace Do_An.Areas.Customer.Controllers
{
    public class ThanhToanController : Controller
    {
        Connection cn = new Connection();
        SqlConnection conn = new SqlConnection("Data Source = MINHDINH; Initial Catalog = TMDT; Integrated Security = True; MultipleActiveResultSets=True;Application Name = EntityFramework");

        THANHTOAN dh = new THANHTOAN();
        // GET: ThanhToan
        DONHANG setDH = new DONHANG();
        [HttpGet]
        public ActionResult Index(string sdt)
        {
            //list sản phẩm
            ViewBag.PhuongThucTT = "Thanh Toán Khi Nhận Sản Phẩm";
            var sanpham = cn.XemGioHangs.SqlQuery("XemGioHang @SDT", new SqlParameter("@SDT", sdt)).ToList();
            ViewBag.GioHang = sanpham;
            //thanh toán
            float tt = 0;

            foreach (var i in sanpham)
            {
                tt += (Convert.ToInt32(i.Gia) * ((float)(100 - Convert.ToInt32(i.Discount)) / 100)) * Convert.ToInt32(i.SoLuong);
            }
            ViewBag.ThanhToan = tt;
            CustomerModel dao = new CustomerModel();
            ViewBag.Detail = dao.infoCustomer(sdt);
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(THANHTOAN donhang, string diachi, string pttt, string sdt )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (donhang.PhuongThucTT == null)
                        ViewBag.PhuongThucTT = "Thanh Toán Khi Nhận Sản Phẩm";
                    else
                        ViewBag.PhuongThucTT = donhang.PhuongThucTT.Replace("{ value = ", "").Replace(" }", "");
                    var sanpham = cn.XemGioHangs.SqlQuery("XemGioHang @SDT", new SqlParameter("@SDT", sdt)).ToList();
                    ViewBag.GioHang = sanpham;
                    //thanh toán
                    float tt = 0;
                    if (donhang.DiaChi == null)
                        ViewBag.DiaChi = ViewBag.GioHang[0].DiaChi;
                    ViewBag.DiaChi = donhang.DiaChi;
                    foreach (var i in sanpham)
                    {
                        tt += (Convert.ToInt32(i.Gia) * ((float)(100 - Convert.ToInt32(i.Discount)) / 100)) * Convert.ToInt32(i.SoLuong);
                    }
                    ViewBag.ThanhToan = tt;
                    if (donhang.ChuThich == null)
                        donhang.ChuThich = "không";
                    List<string> temp = ListSL(sdt);
                    if (pttt == "Thanh toán khi nhận hàng")
                    {
                        if (CheckSL(temp)) 
                        {
                            dh.Mua_Update(sdt, diachi, (int)tt,"Chưa thanh toán", donhang.ChuThich);

                            string maDH = Configuration.MaDH;

                            CustomerModel cus = new CustomerModel();
                            OrderModel ord = new OrderModel();
                            ViewBag.infoCustomer = cus.infoCustomer(sdt); //Lấy thông tin khách hàng

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
                                content = content.Replace("{{Address}}", ord.addressOrderbySDT(sdt));
                                content = content.Replace("{{IDOrder}}", maDH + " <i>(Chưa thanh toán)</i>");
                                content = content.Replace("{{Order}}", noidung);
                                content = content.Replace("{{Total}}", Convert.ToInt32(ViewBag.ThanhToan).ToString("N0"));
                                content = content.Replace("{{Notice}}", notice);
                                toEmail = item.Email;
                            }

                            new MailHelper().SendMail(toEmail, "⌚ Đặt thành công đơn hàng [" + maDH + "] NOLOGO Shop", content);
                            TempData["Alert-Message"] = "Đặt hàng thành công";
                        }
                        return View(donhang);
                    }
                    else
                    {
                        if (CheckSL(temp))
                        {
                            Configuration.SDT = sdt;
                            Configuration.DiaChi = diachi;
                            Configuration.ChuThich = donhang.ChuThich;
                            Configuration.tong = Convert.ToInt32(ViewBag.ThanhToan);
                            return RedirectToAction("PaymentWithPaypal", "Pay", new { @sdt = sdt });
                        }
                    }
                }

                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }
            catch
            {
                return View(donhang);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(string Address, string pttt, THANHTOAN donhang, string sdt )
        {
            var sanpham = cn.XemGioHangs.SqlQuery("XemGioHang @SDT", new SqlParameter("@SDT", sdt)).ToList();
            ViewBag.GioHang = sanpham;
            //thanh toán
            int tt = 0;
            ViewBag.DiaChi = Address;
            donhang.PhuongThucTT = pttt;
            ViewBag.PhuongThucTT = pttt;
            foreach (var i in sanpham)
            {
                tt += Convert.ToInt32(i.Gia) * Convert.ToInt32(i.SoLuong);
            }
            ViewBag.ThanhToan = tt;
            if (ModelState.IsValid)
            {
                // xử lý cho button XacNhan
                dh.Update_DiaChi(Address, sdt);
                return View("Index", donhang);
            }
            return View("Index", donhang);
        }
        public List<string> ListSL(string sdt)
        {
            List<string> lst = new List<string>();
            SqlParameter phone = new SqlParameter("@sdt", sdt);
            SqlCommand cmd = new SqlCommand("Select SP.TenSP, GH.SoLuong as SLGH, SP.SoLuong as SPSL from GIOHANG GH, SANPHAM SP  where SP.MaSP = GH.MaSP and GH.SDT = @sdt", conn);
            cmd.Parameters.Add(phone);
            conn.Open();
            DataTable dt = new DataTable();
            using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            {
                sda.Fill(dt);
                conn.Close();
            }
            int i = 0;
            foreach (DataRow item in dt.Rows)
            {
                if (Convert.ToInt32(item["SLGH"].ToString()) > Convert.ToInt32(item["SPSL"].ToString()))
                {
                    lst.Add("hết hàng");
                    TempData["CheckSL" + i] = item["TenSP"].ToString() + " Đã Hết Hàng";
                }
                else
                    lst.Add("còn hàng");
                i++;
            }
            return lst;
        }
        public bool CheckSL(List<string> lst)
        {
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i] == "hết hàng")
                    return false;
            }
            return true;
        }

    }
}