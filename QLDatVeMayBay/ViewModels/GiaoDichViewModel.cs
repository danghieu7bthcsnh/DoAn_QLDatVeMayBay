using System;

namespace QLDatVeMayBay.ViewModels
{
    public class GiaoDichViewModel
    {
        public int IDThanhToan { get; set; }

        public string HoTenNguoiDung { get; set; } = string.Empty;

        public string TenDangNhap { get; set; } = string.Empty;

        public int IDVe { get; set; }

        public decimal SoTien { get; set; }

        public string PhuongThuc { get; set; } = string.Empty;

        public DateTime ThoiGianGiaoDich { get; set; }

        public string TrangThaiThanhToan { get; set; } = string.Empty;
    }
}
