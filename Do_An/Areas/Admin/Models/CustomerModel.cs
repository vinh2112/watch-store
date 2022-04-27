using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Do_An.Areas.Admin.Models
{
    public class CustomerModel
    {
        MainDbContext db = null;
        public CustomerModel()
        {
            db = new MainDbContext();
        }
        public IEnumerable<INFORMATION> ListAllCustomer()
        {
            return db.Database.SqlQuery<INFORMATION>("AllCustomer").ToList();
        }
        public string countCustomer()
        {
            return db.Database.SqlQuery<INFORMATION>("AllCustomer").ToList().Count().ToString();
        }
        public IEnumerable<INFORMATION> infoCustomer(string SDT)
        {
            return db.INFORMATION.Where(x => x.SDT == SDT).OrderBy(x => x.TenKH);
        }
        public void changeInfo(INFORMATION entity)
        {
            object[] sqlparams =
            {
                new SqlParameter("@SDT", entity.SDT),
                new SqlParameter("@TenKH", entity.TenKH),
                new SqlParameter("@DiaChi", entity.DiaChi),
                new SqlParameter("@Email", entity.Email)
            };
            db.Database.ExecuteSqlCommand("SuaINFORMATION @SDT, @TenKH, @DiaChi, @Email", sqlparams);
        }
    }
}