using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.Models
{
    public class LoaiMayBay
    {
        [Key]
<<<<<<< Updated upstream
        [StringLength(50)]
        public string LoaiMayBayId { get; set; } = string.Empty;
=======
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoaiMayBayId { get; set; }
>>>>>>> Stashed changes

        [Required]
        public int TongSoGhe { get; set; }

        [StringLength(255)]
        public string? MoTa { get; set; }
<<<<<<< Updated upstream
=======
        public ICollection<MayBay> MayBays { get; set; } = new List<MayBay>();
>>>>>>> Stashed changes
    }
}
