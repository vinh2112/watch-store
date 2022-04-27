using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Do_An.Areas.Admin.Models
{
    public class BrandModel
    {
        MainDbContext db = null;
        public BrandModel()
        {
            db = new MainDbContext();
        }
        public IEnumerable<BRAND> infoBrand(string maTh)
        {
            return db.BRANDs.Where(x => x.MaTH == maTh).OrderBy(x => x.MaTH);
        }

        public IEnumerable<BRAND> ListAllBrand()
        {
            return db.BRANDs.OrderBy(x => x.MaTH);
        }
        public bool updateBrand(BRAND entity)
        {
            object[] sqlparams =
            {
                    new SqlParameter("@MaTH", entity.MaTH),
                    new SqlParameter("@TenTH", entity.TenTH),
                    new SqlParameter("@DiaChi", entity.DiaChiTH),
                    new SqlParameter("@SDT", entity.SDT),
                    new SqlParameter("@HinhAnh", entity.HinhAnh),
            };
            try
            {

                db.Database.ExecuteSqlCommand("SuaBRAND @MaTH,@TenTH,@DiaChi,@SDT,@HinhAnh", sqlparams);
                return true;
            }
            catch { }
            return false;
        }
        public bool insertBrand(BRAND entity)
        {
            bool check = false;
            int maTH = 1;
            while (!check)
            {
                if (maTH < 10)
                {
                    entity.MaTH = "TH0" + maTH.ToString();
                }
                else
                {
                    entity.MaTH = "TH" + maTH.ToString();
                }
                try
                {
                    db.BRANDs.Add(entity);
                    db.SaveChanges();
                    check = true;
                    return true;
                }
                catch { }
                maTH++;
            }

            return false;
        }
    }
}