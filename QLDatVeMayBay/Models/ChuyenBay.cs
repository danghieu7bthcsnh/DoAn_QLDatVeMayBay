using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDatVeMayBay.Models
{
    public class ChuyenBay
    {
        [Key]
        public int IDChuyenBay { get; set; }

        public int IDMayBay { get; set; }
        [ForeignKey("IDMayBay")]
        public MayBay? MayBay { get; set; }

        public int SanBayDi { get; set; }
        public int SanBayDen { get; set; }

        [ForeignKey("SanBayDi")]
        public SanBay? SanBayDiInfo { get; set; }

        [ForeignKey("SanBayDen")]
        public SanBay? SanBayDenInfo { get; set; }

        public DateTime GioCatCanh { get; set; }
        public DateTime GioHaCanh { get; set; }

        public decimal GiaVe { get; set; }

        [StringLength(50)]
        public string? TinhTrang { get; set; }
    }
}
