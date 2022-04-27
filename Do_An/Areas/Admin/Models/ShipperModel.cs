using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Do_An.Areas.Admin.Models
{
    public class ShipperModel
    {
        MainDbContext db = null;

        public ShipperModel()
        {
            db = new MainDbContext();
        }

        public IEnumerable<SHIPPER> getShipper()
        {
            return db.Database.SqlQuery<SHIPPER>("AllShipper").ToList();
        }

        public IEnumerable<SHIPPER> getInfoShipper(string SDT)
        {
            object sqlparam = new SqlParameter("@SDT", SDT);
            return db.Database.SqlQuery<SHIPPER>("InfoShipper @SDT", sqlparam).ToList();
        }
        public bool updateShipepr(SHIPPER sp)
        {
            object[] sqlparams =
            {
                new SqlParameter("@SDT", sp.SDT),
                new SqlParameter("@TenShipper", sp.TenKH),
                new SqlParameter("@DiaChi",sp.DiaChi),
                new SqlParameter("@Email",sp.Email),
                new SqlParameter("@CMND", sp.CMND)
            };


            try
            {
                db.Database.ExecuteSqlCommand("updateShipper @SDT, @TenShipper, @DiaChi, @Email, @CMND", sqlparams);
                return true;
            }
            catch { }
            return false;
        }
        public bool insertShipper(SHIPPER sp, string PassW)
        {
            object[] sqlparams =
            {
                new SqlParameter("@SDT", sp.SDT),
                new SqlParameter("@TenShipper", sp.TenKH),
                new SqlParameter("@DiaChi",sp.DiaChi),
                new SqlParameter("@Email",sp.Email),
                new SqlParameter("@CMND", sp.CMND),
                new SqlParameter("@PassW", PassW)
            };

            try
            {
                db.Database.ExecuteSqlCommand("insertShipper @SDT, @TenShipper, @DiaChi, @Email, @CMND, @PassW", sqlparams);
                return true;
            }
            catch { }
            return false;
        }
    }
}