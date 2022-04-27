namespace Do_An.Frameworks
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data.SqlClient;

    [Table("GIOHANG")]
    public partial class GIOHANG
    {
        public static string cn = @"Data Source = MINHDINH; Initial Catalog = TMDT; Integrated Security = True; MultipleActiveResultSets=True;Application Name = EntityFramework";
        SqlConnection conn = new SqlConnection(cn);
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int STT { get; set; }

        [StringLength(10)]
        public string SDT { get; set; }
        [StringLength(50)]
        public string tenSP
        {
            get
            {
                string tensp = "";
                SqlCommand text = new SqlCommand("Select * From SANPHAM Where MaSP='" + MaSP + "'", conn);
                conn.Open();
                SqlDataReader data = text.ExecuteReader();
                while (data.Read())
                {
                    tensp = (string)data["TenSP"] + " " + (string)data["MauSac"] + " " + (string)data["KichThuoc"];
                }
                conn.Close();
                return tensp;
            }
        }

        [StringLength(50)]
        public string MaSP { get; set; }

        public int? SoLuong { get; set; }
        public float DonGia
        {
            get
            {
                float dongia = 0;
                SqlCommand text = new SqlCommand("Select Gia,Discount From SANPHAM Where MaSP='" + MaSP + "'", conn);
                conn.Open();
                SqlDataReader data = text.ExecuteReader();
                while (data.Read())
                {
                    dongia = (int)data["Gia"] * ((float)(100 - (int)data["Discount"]) / 100);
                }
                conn.Close();
                return dongia;

            }
        }
        public string SPimg
        {
            get
            {
                string Hinh = "";
                SqlCommand text = new SqlCommand("Select HinhAnh From SANPHAM Where MaSP='" + MaSP + "'", conn);
                conn.Open();
                SqlDataReader data = text.ExecuteReader();
                while (data.Read())
                {
                    Hinh = (string)data["HinhAnh"];
                }
                conn.Close();
                return Hinh;

            }

        }

        public float ThanhTien
        {
            get
            {
                return DonGia * (float)SoLuong;
            }
        }

        public virtual SANPHAM SANPHAM { get; set; }

        public virtual INFORMATION INFORMATION { get; set; }
    }
}
