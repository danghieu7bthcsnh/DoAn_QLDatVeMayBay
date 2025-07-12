using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.ViewModels
{
    public class DoiMatKhauViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string MatKhauMoi { get; set; }

        [Compare("MatKhauMoi")]
        public string XacNhanMatKhauMoi { get; set; }
    }
}
