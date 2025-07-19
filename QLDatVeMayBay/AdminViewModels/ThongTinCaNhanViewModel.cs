using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.AdminViewModels
{
    public class ThongTinCaNhanViewModel
    {
        public string TenDangNhap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [RegularExpression(@"^\+?\d{9,15}$", ErrorMessage = "Số điện thoại phải gồm 9 đến 15 chữ số, có thể bắt đầu bằng dấu +")]
        public string? SoDienThoai { get; set; }

        [RegularExpression(@"^(Nam|Nữ|Khác)?$", ErrorMessage = "Giới tính phải là Nam, Nữ hoặc Khác")]
        public string? GioiTinh { get; set; }

        [StringLength(100, ErrorMessage = "Quốc tịch tối đa 100 ký tự")]
        public string? QuocTich { get; set; }

        [RegularExpression(@"^\d{9,12}$", ErrorMessage = "CCCD/CMND phải gồm 9 đến 12 chữ số")]
        public string? CCCD { get; set; }
    }
}
