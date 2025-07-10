using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.Models
{
    public class SanBay
    {
        [Key]
        public int IDSanBay { get; set; }

        [StringLength(100)]
        public string TenSanBay { get; set; } = string.Empty;

        [StringLength(100)]
        public string DiaDiem { get; set; } = string.Empty;
    }
}
