using System;
using System.ComponentModel.DataAnnotations;
using QLDatVeMayBay.Models.Entities;

namespace QLDatVeMayBay.Models.ViewModels
{
    public class TheThanhToanViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Phải chọn người dùng.")]
        public int NguoiDungId { get; set; }

        [Required(ErrorMessage = "Loại thẻ/ví là bắt buộc.")]
        public LoaiTheLoaiVi? Loai { get; set; }

        // Thông tin thẻ ngân hàng
        public string? SoThe { get; set; }
        public string? HieuLuc { get; set; }
        public string? CVV { get; set; }
        public string? TenTrenThe { get; set; }

        // Thông tin ví điện tử
        public string? TenVi { get; set; }
        public string? EmailLienKet { get; set; }
        public string? TenHienThi { get; set; }
        public string? SoDienThoai { get; set; }

        [DataType(DataType.Date)]
        public DateTime? NgayLienKet { get; set; }
        public List<TheThanhToan>? DanhSach { get; set; }  // danh sách hiện có
        public TheThanhToan? TheMoi { get; set; }
    }
}
