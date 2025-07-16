using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDatVeMayBay.Models
{
    public class VeMayBay
    {
        [Key]
        public int IDVe { get; set; }

        [Required]
        public int IDNguoiDung { get; set; }

        [Required]
        public int IDChuyenBay { get; set; }

        [Required]
        public int IDGhe { get; set; }

        [ForeignKey("IDNguoiDung")]
        public NguoiDung? NguoiDung { get; set; }

        [ForeignKey("IDChuyenBay")]
        public ChuyenBay? ChuyenBay { get; set; }

        [ForeignKey("IDGhe")]
        public GheNgoi? Ghe { get; set; }

        public DateTime ThoiGianDat { get; set; } = DateTime.Now;

        [StringLength(50)]
        public string? TrangThaiVe { get; set; } = "Chưa thanh toán";
        public string? HangGhe { get; set; }
        public string? LoaiVe { get; set; }
    }
}
