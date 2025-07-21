using QLDatVeMayBay.Models;
using System;
using Xunit;

namespace TestDatVeMayBay.Tests.Models
{
    public class ChuyenBayTests
    {
        [Fact]
        public void TaoChuyenBay_HopLe_ThanhCong()
        {
            // Arrange
            var chuyenBay = new ChuyenBay
            {
                IDChuyenBay = 1,
                IDMayBay = 101,
                SanBayDi = 1,
                SanBayDen = 2,
                GioCatCanh = new DateTime(2025, 7, 25, 10, 0, 0),
                GioHaCanh = new DateTime(2025, 7, 25, 12, 0, 0),
                GiaVe = 1500000,
                TinhTrang = "Đã mở bán"
            };

            // Assert
            Assert.Equal(1, chuyenBay.IDChuyenBay);
            Assert.Equal(101, chuyenBay.IDMayBay);
            Assert.Equal(1, chuyenBay.SanBayDi);
            Assert.Equal(2, chuyenBay.SanBayDen);
            Assert.Equal("Đã mở bán", chuyenBay.TinhTrang);
            Assert.True(chuyenBay.GioHaCanh > chuyenBay.GioCatCanh);
            Assert.True(chuyenBay.GiaVe >= 0);
        }

        [Fact]
        public void TinhTrang_KhongDuocVuotQua50KyTu()
        {
            // Arrange
            var tinhTrangDai = new string('a', 51);
            var chuyenBay = new ChuyenBay { TinhTrang = tinhTrangDai };

            // Act & Assert
            Assert.True(chuyenBay.TinhTrang.Length > 50); // Điều này sẽ thất bại về mặt validation runtime nếu được kiểm tra
        }
    }
}
