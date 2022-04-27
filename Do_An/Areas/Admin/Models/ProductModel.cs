using Do_An.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Data.SqlClient;

namespace Do_An.Areas.Admin.Models
{
    public class ProductModel
    {
        MainDbContext db = null;
        public ProductModel()
        {
            db = new MainDbContext();
        }
        //Table SanPham
        public IEnumerable<SANPHAM> ListAll()
        {
            return db.SANPHAMs.OrderBy(x => x.MaSP);
        }
        public IEnumerable<SANPHAM> ListAlllowQuantity()
        {
            return db.SANPHAMs.Where(x => x.SoLuong < 20).OrderBy(x => x.MaSP);
        }

        public IEnumerable<SANPHAM> ListAllbyBrand(string maTH)
        {
            return db.SANPHAMs.Where(x => x.MaTH == maTH).OrderBy(x => x.MaSP);
        }

        public IEnumerable<SANPHAM> ListAllbyCategory(string maDM)
        {
            return db.SANPHAMs.Where(x => x.MaDM == maDM).OrderBy(y => y.MaSP);
        }
        public string countProduct()
        {
            return db.SANPHAMs.Count().ToString();
        }
        public IEnumerable<SANPHAM> infoProduct(string maSP)
        {
            return db.SANPHAMs.Where(x => x.MaSP == maSP).OrderBy(x => x.MaSP);
        }
        public bool updateProduct(SANPHAM entity)
        {
            object[] sqlparams =
            {
                    new SqlParameter("@MaSP", entity.MaSP),
                    new SqlParameter("@TenSP", entity.TenSP),
                    new SqlParameter("@MauSac", entity.MauSac),
                    new SqlParameter("@KichThuoc", entity.KichThuoc),
                    new SqlParameter("@Gia", entity.Gia*1000),
                    new SqlParameter("@HinhAnh", entity.HinhAnh),
                    new SqlParameter("@SoLuong", entity.SoLuong),
                    new SqlParameter("@MaDM", entity.MaDM),
                    new SqlParameter("@TinhTrang", entity.TinhTrang),
                    new SqlParameter("@Discount", entity.Discount)
            };
            try
            {

                db.Database.ExecuteSqlCommand("SuaSANPHAM @MaSP,@TenSP,@MauSac,@KichThuoc,@Gia,@HinhAnh,@SoLuong,@MaDM,@TinhTrang,@Discount", sqlparams);
                return true;
            }
            catch { }
            return false;
        }
        public bool insertProduct(SANPHAM entity)
        {
            bool check = false;
            int maSP = 1;
            entity.Gia *= 1000;
            while(!check)
            {
                if(maSP < 10)
                {
                    entity.MaSP = "SP0" + maSP.ToString() + entity.MaTH;
                }
                else
                {
                    entity.MaSP = "SP" + maSP.ToString() + entity.MaTH;
                }
                try
                {
                    db.SANPHAMs.Add(entity);
                    db.SaveChanges();
                    check = true;
                    return true;
                }
                catch { }
                maSP++;
            }
            
            return false;
        }

        public void deleteProduct(string maSP,string type)
        {
            object sqlparam = new SqlParameter("@MaSP", maSP);
            try
            {
                if(type == "Invisible")
                {
                    db.Database.ExecuteSqlCommand("AnSANPHAM @MaSP", sqlparam);
                }
                else
                {
                    db.Database.ExecuteSqlCommand("HienSANPHAM @MaSP", sqlparam);
                }
            }
            catch { }
        }
    }
}