using QLDatVeMayBay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace TestDatVeMayBay.Tests.Models
{
    public class DoiMatKhauTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            Validator.TryValidateObject(model, context, results, validateAllProperties: true);
            return results;
        }

        [Fact]
        public void DoiMatKhau_HopLe_ValidationPass()
        {
            // Arrange
            var doiMatKhau = new DoiMatKhau
            {
                Email = "user@example.com",
                MatKhauMoi = "12345678",
                XacNhanMatKhauMoi = "12345678"
            };

            // Act
            var results = ValidateModel(doiMatKhau);

            // Assert
            Assert.Empty(results); // Không có lỗi validation
        }

        [Fact]
        public void DoiMatKhau_ThieuEmail_ValidationError()
        {
            var doiMatKhau = new DoiMatKhau
            {
                Email = null,
                MatKhauMoi = "abc123",
                XacNhanMatKhauMoi = "abc123"
            };

            var results = ValidateModel(doiMatKhau);

            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        [Fact]
        public void DoiMatKhau_MatKhauVaXacNhanKhacNhau_BaoLoiCompare()
        {
            var doiMatKhau = new DoiMatKhau
            {
                Email = "user@example.com",
                MatKhauMoi = "12345678",
                XacNhanMatKhauMoi = "khac1234"
            };

            var results = ValidateModel(doiMatKhau);

            Assert.Contains(results, r => r.MemberNames.Contains("XacNhanMatKhauMoi"));
        }
    }
}
