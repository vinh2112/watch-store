using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Do_An.Areas.Admin.Models
{
    public class DoanhThuModel
    {
        MainDbContext db = null;
        public DoanhThuModel()
        {
            db = new MainDbContext();
        }

        public string DoanhThu()
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            object[] sqlParams =
            {
                new SqlParameter("@Thang",month),
                new SqlParameter("@Nam",year)
            };
            try
            {
                return db.Database.SqlQuery<int>("DoanhThu @Thang,@Nam", sqlParams).SingleOrDefault().ToString();
            }
            catch { return "0"; }
        }
        public int DoanhThuThang(int month)
        {
            int year = DateTime.Now.Year;

            object[] sqlParams =
            {
                new SqlParameter("@Thang",month),
                new SqlParameter("@Nam",year)
            };
            try
            {
                return db.Database.SqlQuery<int>("DoanhThu @Thang,@Nam", sqlParams).SingleOrDefault();
            }
            catch { return 0; }
        }
    }
}