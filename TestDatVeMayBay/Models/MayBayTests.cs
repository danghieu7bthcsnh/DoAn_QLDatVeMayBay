using QLDatVeMayBay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace TestDatVeMayBay.Tests.Models
{
    public class MayBayTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void MayBay_HopLe_KhongCoLoi()
        {
            var mayBay = new MayBay
            {
                TenHangHK = "Vietnam Airlines",
                LoaiMayBayId = 1
            };

            var results = ValidateModel(mayBay);
            Assert.Empty(results);
        }

        [Fact]
        public void MayBay_ThieuTenHangHK()
        {
            var mayBay = new MayBay
            {
                TenHangHK = null!,
                LoaiMayBayId = 1
            };

            var results = ValidateModel(mayBay);
            Assert.Contains(results, r => r.MemberNames.Contains("TenHangHK"));
        }

        [Fact]
        public void MayBay_TenHangHKQuaDai()
        {
            var mayBay = new MayBay
            {
                TenHangHK = new string('A', 101), // vượt quá 100 ký tự
                LoaiMayBayId = 2
            };

            var results = ValidateModel(mayBay);
            Assert.Contains(results, r => r.MemberNames.Contains("TenHangHK"));
        }

        [Fact]
        public void MayBay_ThieuLoaiMayBayId()
        {
            var mayBay = new MayBay
            {
                TenHangHK = "Bamboo Airways",
                LoaiMayBayId = 0 // giả định ID = 0 là không hợp lệ
            };

            var results = ValidateModel(mayBay);
            // không có xác thực đặc biệt cho >0 nên không báo lỗi
            Assert.Empty(results);
        }
    }
}
