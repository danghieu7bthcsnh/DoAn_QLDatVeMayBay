using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.Models
{
    public class TaiKhoan
    {
        [Key]
        [Required]
        [StringLength(50)]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string MatKhau { get; set; } = string.Empty;

        [StringLength(20)]
        public string VaiTro { get; set; } = "KhachHang";

        [StringLength(20)]
        public string TrangThaiTK { get; set; } = "ChuaKichHoat";

        // ✅ Ngày tạo tài khoản
        public DateTime? NgayTao { get; set; } = DateTime.Now;
        public NguoiDung? NguoiDung { get; set; } // Navigation
    }
}