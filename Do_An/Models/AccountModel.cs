using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Do_An.Models
{
    public class AccountModel
    {
        private MainDbContext context = null;
        public AccountModel()
        {
            context = new MainDbContext();
        }
        public bool Login(string username, string password)
        {
            object[] sqlParams =
            {
                new SqlParameter("@Username",username), 
                new SqlParameter("@Password", password)
            };
            var res = context.Database.SqlQuery<bool>("DANGNHAP @Username,@Password",sqlParams).SingleOrDefault();
            return res;
        }
        public bool AdminLogin(string username, string password)
        {
            object[] sqlParams =
            {
                new SqlParameter("@Username",username),
                new SqlParameter("@Password", password)
            };
            var res = context.Database.SqlQuery<bool>("Admin_DANGNHAP @Username,@Password", sqlParams).SingleOrDefault();
            return res;
        }
        public bool Signup(string SDT, string Password, string HoTenKH, string DiaChi, string Email)
        {
            object[] sqlParams = 
            { 
                new SqlParameter("@SDT", SDT),
                new SqlParameter("@HoTenKH", HoTenKH),
                new SqlParameter("@DiaChi", DiaChi),
                new SqlParameter("@Email", Email),
                new SqlParameter("@Pwd",Password)
            };
            try
            {
                context.Database.ExecuteSqlCommand("DANGKY @SDT,@HoTenKH,@DiaChi,@Email,@Pwd", sqlParams);
                return true;
            }
            catch { }
            return false;
        }
        public bool CheckExist(string SDT)
        {
            var res = context.Database.SqlQuery<bool>("Check_SDT @SDT", new SqlParameter("@SDT", SDT)).SingleOrDefault();
            return res;
        }
        public void ResetPassword(string SDT, string NewPass)
        {
            object[] sqlparams =
            {
                new SqlParameter("@UserN",SDT),
                new SqlParameter("@PassW",NewPass)
            };
            context.Database.ExecuteSqlCommand("DoiMK @UserN,@PassW", sqlparams);
        }
    }
}