using QLDatVeMayBay.Models.Entities;
using QLDatVeMayBay.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.Models.ViewModels
{
    public class TheThanhToanViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Phải chọn người dùng.")]
        public int NguoiDungId { get; set; }

        [Required(ErrorMessage = "Loại thẻ/ví là bắt buộc.")]
        public LoaiTheLoaiVi? Loai { get; set; }

        // ---------- THẺ NGÂN HÀNG ----------
        [RequiredIf("Loai", (int)LoaiTheLoaiVi.TheNganHang, ErrorMessage = "Số thẻ là bắt buộc.")]
        public string? SoThe { get; set; }

        [RequiredIf("Loai", (int)LoaiTheLoaiVi.TheNganHang, ErrorMessage = "Ngày hiệu lực là bắt buộc.")]
        public string? HieuLuc { get; set; }

        [RequiredIf("Loai", (int)LoaiTheLoaiVi.TheNganHang, ErrorMessage = "CVV là bắt buộc.")]
        public string? CVV { get; set; }

        [RequiredIf("Loai", (int)LoaiTheLoaiVi.TheNganHang, ErrorMessage = "Tên trên thẻ là bắt buộc.")]
        public string? TenTrenThe { get; set; }

        // ---------- VÍ ĐIỆN TỬ ----------
        [RequiredIf("Loai", (int)LoaiTheLoaiVi.ViDienTu, ErrorMessage = "Tên ví là bắt buộc.")]
        public string? TenVi { get; set; }

        [RequiredIf("Loai", (int)LoaiTheLoaiVi.ViDienTu, ErrorMessage = "Email liên kết là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string? EmailLienKet { get; set; }

        [RequiredIf("Loai", (int)LoaiTheLoaiVi.ViDienTu, ErrorMessage = "Tên hiển thị là bắt buộc.")]
        public string? TenHienThi { get; set; }

        [RequiredIf("Loai", (int)LoaiTheLoaiVi.ViDienTu, ErrorMessage = "Số điện thoại là bắt buộc.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? SoDienThoai { get; set; }

        [RequiredIf("Loai", (int)LoaiTheLoaiVi.ViDienTu, ErrorMessage = "Ngày liên kết là bắt buộc.")]
        [DataType(DataType.Date)]
        public DateTime? NgayLienKet { get; set; }

        public List<TheThanhToan>? DanhSach { get; set; }
        public TheThanhToan? TheMoi { get; set; }
    }
}
