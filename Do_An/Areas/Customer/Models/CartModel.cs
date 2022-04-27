using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Do_An.Areas.Customer.Models
{
    public class CartModel
    {
        MainDbContext db = null;
        public static string cn = @"Data Source = MINHDINH; Initial Catalog = TMDT; Integrated Security = True; MultipleActiveResultSets=True;Application Name = EntityFramework";
        SqlConnection conn = new SqlConnection(cn);
        public CartModel()
        {
            db = new MainDbContext();
        }
        public void XoaSanPham(string sdt, string maSP)
        {
            SqlCommand cmd = new SqlCommand("Delete From GIOHANG Where SDT='" + sdt + "' and MaSP='" + maSP + "'", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void SuaSoLuong(int stt, int soluong)
        {
            SqlCommand cmd = new SqlCommand("Update GIOHANG SET SoLuong=" + soluong + " Where STT='" + stt + "'", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public int getSL(int stt)
        {
            string maSP = db.GIOHANGs.Find(stt).MaSP.ToString();
            int sl = Int32.Parse(db.SANPHAMs.Find(maSP).SoLuong.ToString());
            return sl;
        }
        public void XoaHet(string sdt)
        {
            SqlCommand cmd = new SqlCommand("Delete From GIOHANG Where SDT='" + sdt + "'", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void ThemGioHang(string sdt, string maSP, int soluong)
        {
            //db.themvaogiohang(maSP, sdt, soluong);       
            object[] sqlparams =
            {
                new SqlParameter("@masp",maSP),
                new SqlParameter("@sdt",sdt),
                new SqlParameter("@soluong",soluong)
            };
            db.Database.ExecuteSqlCommand("themvaogiohang @masp, @sdt, @soluong", sqlparams);
        }
    }
}