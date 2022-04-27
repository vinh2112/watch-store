
using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Do_An.Areas.Shipper.Models
{
    public class OrderModel
    {
        MainDbContext db = null;
        public OrderModel()
        {
            db = new MainDbContext();
        }
        public IEnumerable<DONHANG> ListAllOrder()
        {
            return db.DONHANGs.Where(x => x.TinhTrang == "Đang chờ").OrderBy(x => x.MaDH);
        }
        public IEnumerable<DONHANG> ListAllShipping(string sdt)
        {
            return db.DONHANGs.Where(x => x.Shipper == sdt  && x.TinhTrang =="Đang giao").OrderBy(x => x.MaDH);
        }
        public IEnumerable<DONHANG> ListAllDelivered(string sdt)
        {
            return db.DONHANGs.Where(x => x.Shipper == sdt && x.TinhTrang =="Đã giao" || x.TinhTrang== "Đã hủy").OrderBy(x => x.MaDH);
        }
        public IEnumerable<SHIPPER> InfoShipper(string SDT)
        {
            return db.SHIPPERs.Where(x => x.SDT == SDT);
        }
        
        public IEnumerable<DH_SP> infoDonHang(string MaDH)
        {
            return db.DH_SP.Where(x => x.MaDH == MaDH);
        }
        public bool Update(string proc, string MaDH, string SDT)
        {
            object[] sqlparams =
            {
                new SqlParameter("@Proc", proc),
                new SqlParameter("@MaDH", MaDH),
                new SqlParameter("@SDT", SDT)
            };
            try
            {
                db.Database.ExecuteSqlCommand("@proc @MaDH, @SDT", sqlparams);
                return true;
            }
            catch { }
            return false;
        }
    }
}