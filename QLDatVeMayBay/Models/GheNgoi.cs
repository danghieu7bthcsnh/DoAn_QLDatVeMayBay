using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDatVeMayBay.Models
{
    public class GheNgoi
    {
        [Key]
        public int IDGhe { get; set; }

        public int IDChuyenBay { get; set; }

        [ForeignKey("IDChuyenBay")]
        public ChuyenBay? ChuyenBay { get; set; }

        [StringLength(20)]
        public string HangGhe { get; set; } = string.Empty;

        [StringLength(20)]
        public string TrangThai { get; set; } = "Trống";
    }
}
