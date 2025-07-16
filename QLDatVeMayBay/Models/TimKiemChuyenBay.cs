using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.Models
{
    public class TimKiemChuyenBay
    {
        [Display(Name = "Sân bay đi")]
        public int SanBayDi { get; set; }

        [Display(Name = "Sân bay đến")]
        public int SanBayDen { get; set; }

        [Display(Name = "Ngày cất cánh")]
        [DataType(DataType.Date)]
        public DateTime NgayDi { get; set; }
        public string LoaiVe { get; set; } // "MotChieu" hoặc "KhuHoi"
        public string HangGhe { get; set; } // "PhoThong", "ThuongGia", "HangNhat"

        [Display(Name = "Số người lớn")]
        public int NguoiLon { get; set; }

        [Display(Name = "Trẻ em")]
        public int TreEm { get; set; }

        [Display(Name = "Em bé")]
        public int EmBe { get; set; }
    }
}
