using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.Models
{
    public class DoiMatKhau
    {
        [Required]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string MatKhauMoi { get; set; }

        [Compare("MatKhauMoi")]
        public string XacNhanMatKhauMoi { get; set; }
    }
}
