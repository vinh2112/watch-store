using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Do_An.Areas.Customer.Models
{
    [Table("XemGioHang")]
    public partial class XemGioHang
    {
        [Key]
        [StringLength(50)]
        public string MaSP { get; set; }

        [StringLength(200)]
        public string TenSP { get; set; }


        public int? Gia { get; set; }

        public int? Discount { get; set; }

        [StringLength(200)]
        public string HinhAnh { get; set; }

        public int? SoLuong { get; set; }

        [StringLength(10)]
        public string SDT { get; set; }

        [StringLength(100)]
        public string TenKH { get; set; }

        [StringLength(200)]
        public string DiaChi { get; set; }

        [StringLength(100)]
        public string Email { get; set; }
    }
}