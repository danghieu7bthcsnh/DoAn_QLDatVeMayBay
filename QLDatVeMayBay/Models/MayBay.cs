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

        [Required]
        [ForeignKey("LoaiMayBay")]
        public string LoaiMayBayId { get; set; } = string.Empty;

        public LoaiMayBay? LoaiMayBay { get; set; }
    }
}
