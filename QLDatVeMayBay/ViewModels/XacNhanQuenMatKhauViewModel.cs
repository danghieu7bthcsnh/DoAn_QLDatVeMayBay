using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.ViewModels
{
    public class XacNhanQuenMatKhauViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string MaXacNhan { get; set; }
    }
}
