using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.Models
{
    public class LoaiMayBay
    {
        [Key]
        [StringLength(50)]
        public string LoaiMayBayId { get; set; } = string.Empty;

        [Required]
        public int TongSoGhe { get; set; }

        [StringLength(255)]
        public string? MoTa { get; set; }
    }
}
