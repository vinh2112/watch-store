using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Do_An.Frameworks;
using System.Data.SqlClient;

namespace Do_An.Areas.Customer.Models
{
    public class Recommand
    {
        private MainDbContext db = new MainDbContext();

        public IEnumerable<SANPHAM> RECOMMAND(string MaSP)
        {
            string math = db.SANPHAMs.Find(MaSP).MaTH;
            object[] sqlParamter =
                {
                new SqlParameter("@MaTH", math),
                new SqlParameter("@masp",MaSP)
            };

            var res = db.Database.SqlQuery<SANPHAM>("RECOMMAND @MaTH, @masp", sqlParamter).ToList();
            return res;
        }
    }
}