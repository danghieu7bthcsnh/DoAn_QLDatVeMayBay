﻿using System.ComponentModel.DataAnnotations;

namespace QLDatVeMayBay.Models
{
    public class ThongTinThanhToan
    {
        public VeMayBay Ve { get; set; }

        [Required(ErrorMessage = "Chọn phương thức thanh toán")]
        public string PhuongThuc { get; set; }

        [Required(ErrorMessage = "Chọn ngân hàng")]
        public string TenNganHang { get; set; }

        [Required(ErrorMessage = "Nhập số tài khoản")]
        public string SoTaiKhoan { get; set; }

        [Required(ErrorMessage = "Nhập tên chủ tài khoản")]
        public string ChuTaiKhoan { get; set; }

        public decimal SoTien { get; set; }
        public string MaOTP { get; set; }
        public ThanhToan ThanhToan { get; set; }

    }
}