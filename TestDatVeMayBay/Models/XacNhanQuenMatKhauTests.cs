using QLDatVeMayBay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace TestDatVeMayBay.Tests.Models
{
    public class XacNhanQuenMatKhauTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void XacNhanQuenMatKhau_HopLe_KhongCoLoi()
        {
            var model = new XacNhanQuenMatKhau
            {
                Email = "example@gmail.com",
                MaXacNhan = "123456"
            };

            var results = ValidateModel(model);
            Assert.Empty(results);
        }

        [Fact]
        public void XacNhanQuenMatKhau_ThieuEmail_BaoLoi()
        {
            var model = new XacNhanQuenMatKhau
            {
                Email = null,
                MaXacNhan = "123456"
            };

            var results = ValidateModel(model);
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        [Fact]
        public void XacNhanQuenMatKhau_ThieuMaXacNhan_BaoLoi()
        {
            var model = new XacNhanQuenMatKhau
            {
                Email = "example@gmail.com",
                MaXacNhan = null
            };

            var results = ValidateModel(model);
            Assert.Contains(results, r => r.MemberNames.Contains("MaXacNhan"));
        }

        [Fact]
        public void XacNhanQuenMatKhau_ThieuCaHai_BaoLoi()
        {
            var model = new XacNhanQuenMatKhau();

            var results = ValidateModel(model);
            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
            Assert.Contains(results, r => r.MemberNames.Contains("MaXacNhan"));
        }
    }
}
