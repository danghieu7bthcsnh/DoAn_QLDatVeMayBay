using QLDatVeMayBay.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDatVeMayBay.Models
{
    public class VeMayBay
    {
        [Key]
        public int IDVe { get; set; }

        public int IDNguoiDung { get; set; }
        public int IDChuyenBay { get; set; }
        public int IDGhe { get; set; }

        [ForeignKey("IDNguoiDung")]
        public NguoiDung? NguoiDung { get; set; }

        [ForeignKey("IDChuyenBay")]
        public ChuyenBay? ChuyenBay { get; set; }

        [ForeignKey("IDGhe")]
        public GheNgoi? Ghe { get; set; }

        public DateTime ThoiGianDat { get; set; }

        [StringLength(50)]
        public string? TrangThaiVe { get; set; } = "Chưa thanh toán";

        public string? HangGhe { get; set; }
        public string? LoaiVe { get; set; }

        // ✅ FIX: độ dài phải giống với TheThanhToan.Id (450)
        [MaxLength(450)]
        public string? TheThanhToanId { get; set; }

        [ForeignKey("TheThanhToanId")]
        public TheThanhToan? TheThanhToan { get; set; }

        public ThanhToan? ThanhToan { get; set; }
    }
}
