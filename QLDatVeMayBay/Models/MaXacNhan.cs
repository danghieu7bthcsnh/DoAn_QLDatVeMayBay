using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.Models
{
    public class MaXacNhan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required]
        public string Ma { get; set; } = string.Empty;

        public DateTime ThoiGianHetHan { get; set; }
    }
}
