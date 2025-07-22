using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDatVeMayBay.Models.Entities
{
    public class TheThanhToan
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // ✅ Sửa dòng này: int thay vì string
        [Required]
        [Column("IDNguoiDung")]
        public int NguoiDungId { get; set; }

        [ForeignKey("IDNguoiDung")]
        public NguoiDung? NguoiDung { get; set; }

        [Required]
        public LoaiTheLoaiVi Loai { get; set; }

        public string? SoThe { get; set; }
        public string? HieuLuc { get; set; }
        public string? CVV { get; set; }
        public string? TenTrenThe { get; set; }

        public string? TenVi { get; set; }
        public string? EmailLienKet { get; set; }
        public string? TenHienThi { get; set; }
        public string? SoDienThoai { get; set; }
        public DateTime? NgayLienKet { get; set; }

        public List<TheThanhToan> DanhSach { get; set; } = new();
    }

    public enum LoaiTheLoaiVi
    {
        TheNganHang = 0,
        ViDienTu = 1
    }
}
