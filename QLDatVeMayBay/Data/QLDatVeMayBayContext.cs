using Microsoft.EntityFrameworkCore;
using QLDatVeMayBay.Models;

namespace QLDatVeMayBay.Data
{
    public class QLDatVeMayBayContext : DbContext
    {
        public QLDatVeMayBayContext()
        {
        }

        public QLDatVeMayBayContext(DbContextOptions<QLDatVeMayBayContext> options) : base(options)
        {
        }

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
        public DbSet<LienKetThe> LienKetThe { get; set; }
        public DbSet<YeuCauHoanTien> YeuCauHoanTien { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình khóa chính
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
                .WithMany()
                .HasForeignKey(v => v.IDNguoiDung)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VeMayBay>()
                .HasOne(v => v.ChuyenBay)
                .WithMany()
                .HasForeignKey(v => v.IDChuyenBay)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ThanhToan>()
                .HasOne(tt => tt.VeMayBay)
                .WithMany()
                .HasForeignKey(tt => tt.IDVe)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MaXacNhan>()
                .HasKey(m => m.Id);

            // Cấu hình kiểu decimal cho tránh bị cảnh báo
            modelBuilder.Entity<ChuyenBay>()
                .Property(cb => cb.GiaVe)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ThanhToan>()
                .Property(tt => tt.SoTien)
                .HasColumnType("decimal(18,2)");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<YeuCauHoanTien>()
                .HasOne(y => y.VeMayBay)
                .WithMany(v => v.YeuCauHoanTiens)
                .HasForeignKey(y => y.IDVe)
                .OnDelete(DeleteBehavior.Restrict); // <- Sửa tại đây

            modelBuilder.Entity<YeuCauHoanTien>()
                .HasOne(y => y.NguoiDung)
                .WithMany()
                .HasForeignKey(y => y.IDNguoiDung)
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }
    }
}
