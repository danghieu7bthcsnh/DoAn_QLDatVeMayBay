using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.ViewModels
{
    public class XacNhanEmailViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập tối đa 50 ký tự")]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mã xác nhận")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Mã xác nhận phải gồm 6 chữ số")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Mã xác nhận phải là 6 chữ số")]
        public string MaXacNhan { get; set; } = string.Empty;
    }
}
