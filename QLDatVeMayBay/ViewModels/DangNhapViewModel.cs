using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.ViewModels
{
    public class DangNhapViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập hoặc email.")]
        [StringLength(100, ErrorMessage = "Không vượt quá 100 ký tự.")]
        [Display(Name = "Tên đăng nhập hoặc Email")]
        public string TenDangNhapOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Mật khẩu không được vượt quá 100 ký tự.")]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; } = string.Empty;

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool GhiNhoDangNhap { get; set; } = false;

        [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
        [Display(Name = "Vai trò")]
        [RegularExpression("^(Admin|KhachHang)$", ErrorMessage = "Vai trò không hợp lệ.")]
        public string VaiTro { get; set; } = "KhachHang";
    }
}