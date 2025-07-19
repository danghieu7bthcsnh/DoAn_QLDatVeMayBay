using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.ViewModels.TaiKhoan
{
    public class ChinhSuaHoSoViewModel
    {
        public int IDNguoiDung { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(20)]
        public string? SoDienThoai { get; set; }

        [StringLength(10)]
        public string? GioiTinh { get; set; }

        [StringLength(50)]
        public string? QuocTich { get; set; }

        [StringLength(20)]
        public string? CCCD { get; set; }
    }
}
