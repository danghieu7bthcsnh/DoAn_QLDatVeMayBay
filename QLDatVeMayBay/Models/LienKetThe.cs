using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDatVeMayBay.Models
{
    public class LienKetThe
    {
        [Key]
        public int IdLienKet { get; set; }

        // Khóa ngoại đến NguoiDung
        public int IDNguoiDung { get; set; }

        [ForeignKey("IDNguoiDung")]
        public NguoiDung NguoiDung { get; set; }

        [Required]
        public string LoaiThe { get; set; } // Visa, Momo, ZaloPay,...

        [Required]
        [StringLength(20)]
        public string SoThe { get; set; }

        [Required]
        public string NganHang { get; set; }
    }

}
