using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Do_An.Areas.Shipper.Models
{
    public class ThongKeModel
    {
        MainDbContext db = null;
        public ThongKeModel()
        {
            db = new MainDbContext();
        }
        public int TotalCancel(string SDT)
        {
            return db.DONHANGs.Where(x => x.Shipper == SDT && x.TinhTrang == "Đã hủy").Count();
        }
        public int TotalAccept(string SDT)
        {
            return db.DONHANGs.Where(x => x.Shipper == SDT && x.TinhTrang == "Đã giao").Count();
        }
        public int Total(string SDT)
        {
            return db.DONHANGs.Where(x => x.Shipper == SDT).Count();
        }
        public List<ThongKeShip> GetThongKe( string SDT)
        {
            DateTime date = DateTime.Now;

            object[] sqlparams =
            {
                new SqlParameter("@Thang" , date.Month),
                new SqlParameter("@Nam", date.Year),
                new SqlParameter("@SDT", SDT)
            };
            return db.Database.SqlQuery<ThongKeShip>("ThongKe @Thang, @Nam, @SDT", sqlparams).ToList();
        }
    }
    public class ThongKeShip
    {
        public DateTime NgayXacNhan { set; get; }
        public int ThongKe { set; get; }
    }
}