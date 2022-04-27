namespace Do_An.Frameworks
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DONHANG")]
    public partial class DONHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DONHANG()
        {
            DH_SP = new HashSet<DH_SP>();
        }
        MainDbContext db = new MainDbContext();
        [Key]
        [StringLength(50)]
        public string MaDH { get; set; }

        [StringLength(10)]
        public string SDT { get; set; }

        [StringLength(200)]
        public string DiaChi { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayThang { get; set; }

        [StringLength(50)]
        public string TinhTrang { get; set; }

        [StringLength(200)]
        public string ChuThich { get; set; }

        public int? TongTien { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayXacNhan { get; set; }

        [StringLength(10)]
        public string Shipper { get; set; }

        public string TenShipper
        {
            get
            {
                if(Shipper != null)
                {
                    string tenSP = "";
                    tenSP = db.INFORMATION.Find(Shipper).TenKH.ToString();
                    return tenSP;
                }
                else
                {
                    return "";
                }
            }
        }

        [StringLength(50)]
        public string ThanhToan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DH_SP> DH_SP { get; set; }

        public virtual INFORMATION INFORMATION { get; set; }

        public virtual SHIPPER SHIPPER1 { get; set; }
    }
}
