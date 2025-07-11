using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.ViewModels
{
    public class QuenMatKhauViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

    }
}
