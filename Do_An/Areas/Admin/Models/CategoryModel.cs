using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Do_An.Areas.Admin.Models
{
    public class CategoryModel
    {
        MainDbContext db = null;
        public CategoryModel()
        {
            db = new MainDbContext();
        }
        public IEnumerable<DANHMUC> ListAllCategory()
        {
            return db.DANHMUCs.OrderBy(x => x.MaDM);
        }
        public IEnumerable<DANHMUC> infoCategory(string maDM)
        {
            return db.DANHMUCs.Where(x => x.MaDM == maDM).OrderBy(x => x.MaDM);
        }
        public bool updateCategory(DANHMUC entity)
        {
            object[] sqlParams =
            {
                new SqlParameter("@MaDM", entity.MaDM),
                new SqlParameter("@TenDM", entity.TenDM)
            };

            try
            {
                db.Database.ExecuteSqlCommand("SuaDANHMUC @MaDM,@TenDM", sqlParams);
                return true;
            }
            catch { }
            return false;
        }
        public bool insertCategory(DANHMUC entity)
        {
            bool check = false;
            int maDM = 1;
            while (!check)
            {
                if (maDM < 10)
                {
                    entity.MaDM = "DM0" + maDM.ToString();
                }
                else
                {
                    entity.MaDM = "DM" + maDM.ToString();
                }
                try
                {
                    db.DANHMUCs.Add(entity);
                    db.SaveChanges();
                    check = true;
                    return true;
                }
                catch { }
                maDM++;
            }
            return false;
        }
    }
}