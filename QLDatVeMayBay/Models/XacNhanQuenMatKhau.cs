using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.Models
{
    public class XacNhanQuenMatKhau
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string MaXacNhan { get; set; }
    }
}
