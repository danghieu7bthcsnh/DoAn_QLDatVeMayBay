using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.Models
{
    public class QuenMatKhau
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
