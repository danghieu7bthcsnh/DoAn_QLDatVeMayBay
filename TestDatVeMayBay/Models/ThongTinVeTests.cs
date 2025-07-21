using QLDatVeMayBay.Models;
using QLDatVeMayBay.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace TestDatVeMayBay.Tests.Models
{
    public class ThongTinVeTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void ThongTinVe_HopLe_KhongCoLoi()
        {
            var ve = new ThongTinVe
            {
                IDNguoiDung = 1,
                HoTen = "Nguyen Van A",
                GioiTinh = "Nam",
                NgaySinh = new DateTime(2000, 1, 1),
                IDChuyenBay = 100,
                TenHangHK = "Vietnam Airlines",
                GioCatCanh = new DateTime(2025, 8, 1, 10, 0, 0),
                GioHaCanh = new DateTime(2025, 8, 1, 12, 0, 0),
                SanBayDi = 1,
                SanBayDen = 2,
                TenSanBayDi = "Nội Bài",
                TenSanBayDen = "Tân Sơn Nhất",
                IDGhe = "12A",
                GiaVe = 1500000,
                QRBase64 = "qrcodebase64string...",
                DanhSachThe = new List<TheThanhToan> { new TheThanhToan { Id = "1", TenHienThi = "Nguyen Van A" } },
                SelectedTheId = "1"
            };

            var results = ValidateModel(ve);

            Assert.Empty(results);
        }

        [Fact]
        public void ThongTinVe_ThieuSelectedTheId_BaoLoiRequired()
        {
            var ve = new ThongTinVe
            {
                IDNguoiDung = 1,
                HoTen = "Nguyen Van A",
                GioiTinh = "Nam",
                NgaySinh = new DateTime(2000, 1, 1),
                IDChuyenBay = 100,
                TenHangHK = "Vietnam Airlines",
                GioCatCanh = new DateTime(2025, 8, 1, 10, 0, 0),
                GioHaCanh = new DateTime(2025, 8, 1, 12, 0, 0),
                SanBayDi = 1,
                SanBayDen = 2,
                TenSanBayDi = "Nội Bài",
                TenSanBayDen = "Tân Sơn Nhất",
                IDGhe = "12A",
                GiaVe = 1500000,
                QRBase64 = "qrcodebase64string...",
                DanhSachThe = new List<TheThanhToan> { new TheThanhToan { Id = "1", TenTrenThe = "Nguyen Van A" } },
                SelectedTheId = null // Thiếu
            };

            var results = ValidateModel(ve);

            Assert.Single(results);
            Assert.Contains(results, r => r.MemberNames.Contains("SelectedTheId"));
        }

        [Fact]
        public void ThongTinVe_GioHaCanh_TruocGioCatCanh_KhongKiemTra()
        {
            var ve = new ThongTinVe
            {
                IDNguoiDung = 1,
                HoTen = "Nguyen Van A",
                GioiTinh = "Nam",
                NgaySinh = new DateTime(2000, 1, 1),
                IDChuyenBay = 100,
                TenHangHK = "Vietnam Airlines",
                GioCatCanh = new DateTime(2025, 8, 1, 12, 0, 0),
                GioHaCanh = new DateTime(2025, 8, 1, 10, 0, 0), // không hợp lý nhưng chưa có ràng buộc
                SanBayDi = 1,
                SanBayDen = 2,
                TenSanBayDi = "Nội Bài",
                TenSanBayDen = "Tân Sơn Nhất",
                IDGhe = "12A",
                GiaVe = 1500000,
                QRBase64 = "qrcodebase64string...",
                DanhSachThe = new List<TheThanhToan> { new TheThanhToan { Id = "1", TenTrenThe = "Nguyen Van A" } },
                SelectedTheId = "1"
            };

            var results = ValidateModel(ve);

            // Không có ràng buộc nên sẽ không báo lỗi
            Assert.Empty(results);
        }
    }
}
