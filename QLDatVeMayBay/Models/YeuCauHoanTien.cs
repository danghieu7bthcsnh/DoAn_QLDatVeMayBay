using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLDatVeMayBay.Models
{
    public class YeuCauHoanTien
    {
        [Key]
        [Required]
        public int IDHoanTien { get; set; }
        public int IDNguoiDung { get; set; }

        [ForeignKey("IDNguoiDung")]
        public NguoiDung? NguoiDung { get; set; }
        public int IDVe { get; set; }

        [ForeignKey("IDVe")]
        public VeMayBay? VeMayBay { get; set; }
        public string TrangThai { get; set; } // Đang chờ, Đang xử lý, Hoàn tất
        public DateTime NgayTao { get; set; }

      
    }

}
A