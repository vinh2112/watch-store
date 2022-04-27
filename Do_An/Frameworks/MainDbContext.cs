namespace Do_An.Frameworks
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MainDbContext : DbContext
    {
        public MainDbContext()
            : base("name=MainDbContext")
        {
        }

        public virtual DbSet<BRAND> BRANDs { get; set; }
        public virtual DbSet<DANHMUC> DANHMUCs { get; set; }
        public virtual DbSet<DH_SP> DH_SP { get; set; }
        public virtual DbSet<DONHANG> DONHANGs { get; set; }
        public virtual DbSet<GIOHANG> GIOHANGs { get; set; }
        public virtual DbSet<INFORMATION> INFORMATION { get; set; }
        public virtual DbSet<SANPHAM> SANPHAMs { get; set; }
        public virtual DbSet<SHIPPER> SHIPPERs { get; set; }
        public virtual DbSet<USER> USERS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DONHANG>()
                .HasMany(e => e.DH_SP)
                .WithOptional(e => e.DONHANG)
                .WillCascadeOnDelete();

            modelBuilder.Entity<INFORMATION>()
                .HasMany(e => e.USERS)
                .WithOptional(e => e.INFORMATION)
                .HasForeignKey(e => e.UserN);

            modelBuilder.Entity<INFORMATION>()
                .HasOptional(e => e.SHIPPER)
                .WithRequired(e => e.INFORMATION);

            modelBuilder.Entity<SHIPPER>()
                .HasMany(e => e.DONHANGs)
                .WithOptional(e => e.SHIPPER1)
                .HasForeignKey(e => e.Shipper);
        }
    }
}
