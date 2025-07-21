using QLDatVeMayBay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace TestDatVeMayBay.Tests.Models
{
    public class GheNgoiTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void GheNgoi_HopLe_ValidationPass()
        {
            var ghe = new GheNgoi
            {
                IDGhe = 1,
                IDChuyenBay = 101,
                HangGhe = "Hạng A",
                TrangThai = "Trống"
            };

            var results = ValidateModel(ghe);

            Assert.Empty(results); // Không có lỗi
        }

        [Fact]
        public void GheNgoi_HangGheQuaDai_BaoLoiStringLength()
        {
            var ghe = new GheNgoi
            {
                IDGhe = 2,
                IDChuyenBay = 102,
                HangGhe = new string('A', 30), // > 20 ký tự
                TrangThai = "Trống"
            };

            var results = ValidateModel(ghe);

            Assert.Contains(results, r => r.MemberNames.Contains("HangGhe"));
        }

        [Fact]
        public void GheNgoi_TrangThaiQuaDai_BaoLoiStringLength()
        {
            var ghe = new GheNgoi
            {
                IDGhe = 3,
                IDChuyenBay = 103,
                HangGhe = "B1",
                TrangThai = new string('T', 50) // > 20 ký tự
            };

            var results = ValidateModel(ghe);

            Assert.Contains(results, r => r.MemberNames.Contains("TrangThai"));
        }
    }
}
