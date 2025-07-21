using QLDatVeMayBay.Models;
using System.Collections.Generic;
using Xunit;

namespace TestDatVeMayBay.Tests.Models
{
    public class DatGheTests
    {
        [Fact]
        public void TaoDatGhe_HopLe_ThanhCong()
        {
            // Arrange
            var datGhe = new DatGhe
            {
                IDChuyenBay = 1001,
                TongSoGhe = 100,
                GheDaDat = new List<int> { 1, 2, 3 }
            };

            // Assert
            Assert.Equal(1001, datGhe.IDChuyenBay);
            Assert.Equal(100, datGhe.TongSoGhe);
            Assert.Contains(1, datGhe.GheDaDat);
            Assert.Equal(3, datGhe.GheDaDat.Count);
        }

        [Fact]
        public void DatGhe_KhongVuotQuaTongSoGhe()
        {
            // Arrange
            var datGhe = new DatGhe
            {
                TongSoGhe = 5,
                GheDaDat = new List<int> { 1, 2, 3, 4, 5 }
            };

            // Act
            bool coVuotGhe = datGhe.GheDaDat.Count > datGhe.TongSoGhe;

            // Assert
            Assert.False(coVuotGhe);
        }

        [Fact]
        public void DatGhe_TrungSoGhe_KhongHopLe()
        {
            // Arrange
            var datGhe = new DatGhe
            {
                TongSoGhe = 10,
                GheDaDat = new List<int> { 1, 2, 3, 3, 4 }
            };

            // Act
            var gheTrung = datGhe.GheDaDat.GroupBy(x => x)
                                .Where(g => g.Count() > 1)
                                .Select(g => g.Key)
                                .ToList();

            // Assert
            Assert.Contains(3, gheTrung);
        }
    }
}
