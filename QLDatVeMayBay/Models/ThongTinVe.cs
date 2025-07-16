namespace QLDatVeMayBay.Models
{
    public class ThongTinVe
    {
        public int IDNguoiDung { get; set; }
        public string HoTen { get; set; }
        public string GioiTinh { get; set; }
        public DateTime NgaySinh { get; set; }

        public int IDChuyenBay { get; set; }
        public string TenHangHK { get; set; }
        public DateTime GioCatCanh { get; set; }
        public DateTime GioHaCanh { get; set; }
        public int SanBayDi { get; set; }
        public int SanBayDen { get; set; }

        public string TenSanBayDi { get; set; }
        public string TenSanBayDen { get; set; }

        public string IDGhe { get; set; }
        public decimal GiaVe { get; set; }
        public string QRBase64 { get; set; } // chứa mã QR (Base64)
    }

}
