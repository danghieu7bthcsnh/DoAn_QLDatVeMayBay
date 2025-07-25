﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDatVeMayBay.Models
{
    public class NguoiDung
    {
        [Key]
        public int IDNguoiDung { get; set; }

        [Required]
        [ForeignKey("TaiKhoan")]
        [StringLength(50)]
        public string TenDangNhap { get; set; } = string.Empty;

        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        [Phone]
        public string? SoDienThoai { get; set; }

        [StringLength(10)]
        public string? GioiTinh { get; set; }

        [StringLength(50)]
        public string? QuocTich { get; set; }

        [StringLength(20)]
        public string? CCCD { get; set; }

        // ✅ Navigation đến tài khoản
        public TaiKhoan? TaiKhoan { get; set; }

        // ✅ Navigation đến vé máy bay đã đặt
        public List<VeMayBay> VeMayBays { get; set; } = new List<VeMayBay>();
    }
}