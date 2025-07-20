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

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(20)]
        public string SoDienThoai { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giới tính không được để trống")]
        [StringLength(10)]
        public string GioiTinh { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quốc tịch không được để trống")]
        [StringLength(50)]
        public string QuocTich { get; set; } = string.Empty;

        [Required(ErrorMessage = "CCCD không được để trống")]
        [StringLength(20)]
        public string CCCD { get; set; } = string.Empty;
    }
}
