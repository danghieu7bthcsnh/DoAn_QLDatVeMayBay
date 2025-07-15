

// ✅ DbContext đã chỉnh Fluent API rõ ràng, tránh FK ảo, quản lý dễ bảo trì
using Microsoft.EntityFrameworkCore;
using QLDatVeMayBay.Models;

namespace QLDatVeMayBay.Data
{
    public class QLDatVeMayBayContext : DbContext
    {
        public QLDatVeMayBayContext(DbContextOptions<QLDatVeMayBayContext> options) : base(options) { }

        public DbSet<TaiKhoan> TaiKhoan { get; set; }
        public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<LoaiMayBay> LoaiMayBay { get; set; }
        public DbSet<MayBay> MayBay { get; set; }
        public DbSet<SanBay> SanBay { get; set; }
        public DbSet<ChuyenBay> ChuyenBay { get; set; }
        public DbSet<GheNgoi> GheNgoi { get; set; }
        public DbSet<VeMayBay> VeMayBay { get; set; }
        public DbSet<ThanhToan> ThanhToan { get; set; }
        public DbSet<MaXacNhan> MaXacNhan { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaiKhoan>()
                .HasKey(t => t.TenDangNhap);

            modelBuilder.Entity<NguoiDung>()
                .HasOne(n => n.TaiKhoan)
                .WithOne(t => t.NguoiDung)
                .HasForeignKey<NguoiDung>(n => n.TenDangNhap)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LoaiMayBay>()
                .HasKey(l => l.LoaiMayBayId);

            modelBuilder.Entity<MayBay>()
                .HasOne(m => m.LoaiMayBay)
                .WithMany()
                .HasForeignKey(m => m.LoaiMayBayId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SanBay>()
                .HasKey(s => s.IDSanBay);

            modelBuilder.Entity<ChuyenBay>()
                .HasOne(cb => cb.MayBay)
                .WithMany()
                .HasForeignKey(cb => cb.IDMayBay)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChuyenBay>()
                .HasOne(cb => cb.SanBayDiInfo)
                .WithMany()
                .HasForeignKey(cb => cb.SanBayDi)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChuyenBay>()
                .HasOne(cb => cb.SanBayDenInfo)
                .WithMany()
                .HasForeignKey(cb => cb.SanBayDen)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GheNgoi>()
                .HasKey(g => g.IDGhe);

            modelBuilder.Entity<GheNgoi>()
                .HasOne(g => g.ChuyenBay)
                .WithMany()
                .HasForeignKey(g => g.IDChuyenBay)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VeMayBay>()
                .HasOne(v => v.NguoiDung)
                .WithMany(nd => nd.VeMayBays)
                .HasForeignKey(v => v.IDNguoiDung)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VeMayBay>()
                .HasOne(v => v.ChuyenBay)
                .WithMany()
                .HasForeignKey(v => v.IDChuyenBay)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VeMayBay>()
                .HasOne(v => v.Ghe)
                .WithMany()
                .HasForeignKey(v => v.IDGhe)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ThanhToan>()
                .HasOne(tt => tt.VeMayBay)
                .WithMany()
                .HasForeignKey(tt => tt.IDVe)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MaXacNhan>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<ChuyenBay>()
                .Property(cb => cb.GiaVe)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ThanhToan>()
                .Property(tt => tt.SoTien)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
