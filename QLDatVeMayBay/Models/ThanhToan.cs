using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDatVeMayBay.Models
{
    public class ThanhToan
    {
        [Key]
        public int IDThanhToan { get; set; }

        public int IDNguoiDung { get; set; }

        [ForeignKey("IDNguoiDung")]
        public NguoiDung ? NguoiDung { get; set; }
        public int IDVe { get; set; }

        [ForeignKey("IDVe")]
        public VeMayBay? VeMayBay { get; set; }

        public decimal SoTien { get; set; }

        [StringLength(50)]
        public string? PhuongThuc { get; set; }

        public DateTime ThoiGianGiaoDich { get; set; }

        [StringLength(50)]
        public string? TrangThaiThanhToan { get; set; }
    }
}
