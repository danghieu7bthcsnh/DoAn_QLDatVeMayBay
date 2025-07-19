using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDatVeMayBay.Models
{
    public class MayBay
    {
        [Key]
        public int IDMayBay { get; set; }

        [Required]
        [StringLength(100)]
        public string TenHangHK { get; set; } = string.Empty;

        // Foreign key - EF hiểu tự động
        public int LoaiMayBayId { get; set; }

        // Navigation property - nên để không nullable
        public LoaiMayBay? LoaiMayBay { get; set; }
    }
}
