using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDatVeMayBay.Models
{
    public class HoanTien
    {
        [Key]
        public int IDHoanTien { get; set; }

        [Required]
        public int IDThanhToan { get; set; }

        [ForeignKey("IDThanhToan")]
        public ThanhToan? ThanhToan { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SoTienHoan { get; set; }

        public DateTime NgayHoanTien { get; set; } = DateTime.Now;

        [StringLength(200)]
        public string? LyDo { get; set; }
    }
}
