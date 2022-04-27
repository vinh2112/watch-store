using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Do_An.Areas.Admin.Models
{
    public class OrderModel
    {
        SqlConnection con = new SqlConnection(@"Data Source=MINHDINH;Initial Catalog=TMDT;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        MainDbContext db = null;
        public OrderModel()
        {
            db = new MainDbContext();
        }
        public string countOrder()
        {
            return db.DONHANGs.Where(x => x.TinhTrang == "Đang chờ").Count().ToString();
        }
        public IEnumerable<DONHANG> ListOrderbySDT(string SDT)
        {
            return db.DONHANGs.Where(x => x.SDT == SDT).OrderBy(x => x.MaDH);
        }
        public IEnumerable<DONHANG> ListAllOrder()
        {
            return db.DONHANGs.OrderBy(x => x.MaDH);
        }
        public IEnumerable<DONHANG> ListAllSuccessOrder()
        {
            return db.DONHANGs.Where(x => x.TinhTrang == "Đã giao").OrderBy(x => x.MaDH);
        }
        public IEnumerable<DONHANG> ListAllWaitingOrder()
        {
            return db.DONHANGs.Where(x => x.TinhTrang == "Đang chờ").OrderBy(x => x.MaDH);
        }
        public IEnumerable<DONHANG> ListAllShippingOrder()
        {
            return db.DONHANGs.Where(x => x.TinhTrang == "Đang giao").OrderBy(x => x.MaDH);
        }
        public IEnumerable<DONHANG> ListAllCanceledOrder()
        {
            return db.DONHANGs.Where(x => x.TinhTrang == "Đã hủy").OrderBy(x => x.MaDH);
        }
        public IEnumerable<DH_SP> infoDonHang(string MaDH)
        {
            return db.DH_SP.Where(x => x.MaDH == MaDH);
        }
        public bool updateOrder(string MaDH, string status)
        {
            object[] sqlparams =
            {
                new SqlParameter("@MaDH",MaDH),
                new SqlParameter("@TinhTrang",status)
            };
            try
            {
                db.Database.ExecuteSqlCommand("updateDONHANG @MaDH, @TinhTrang", sqlparams);
            }
            catch { }
            return false;
        }
        public void editQuantity(string maSP, int SL)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE SANPHAM SET SoLuong = SoLuong + @SL WHERE MaSP = @MaSP", con);
            cmd.Parameters.Add("@MaSP", SqlDbType.VarChar).Value = maSP.Trim();
            cmd.Parameters.Add("@SL", SqlDbType.Int).Value = SL;
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public string addressOrderbySDT(string sdt)
        {
            return db.DONHANGs.Where(x => x.SDT == sdt).Select(x => x.DiaChi).FirstOrDefault().ToString();
        }
        public bool chageInfo(string MaDH, string DiaChi)
        {
            object[] sqlparams =
            {
                new SqlParameter("@MaDH",MaDH),
                new SqlParameter("@DiaChi",DiaChi)
            };
            try
            {
                db.Database.ExecuteSqlCommand("changeDonHang @MaDH,@DiaChi", sqlparams);
                return true;
            }
            catch { }
            return false;
        }
    }
}