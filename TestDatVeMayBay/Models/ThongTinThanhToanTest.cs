using QLDatVeMayBay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace QLDatVeMayBay.Tests
{
    public class ThongTinThanhToanTests
    {
        private List<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void ThongTinThanhToan_InvalidThiếuTruongRequired_ShouldReturnValidationErrors()
        {
            // Arrange
            var model = new ThongTinThanhToan
            {
                PhuongThuc = null,
                TenNganHang = null,
                SoTaiKhoan = null,
                ChuTaiKhoan = null,
                SelectedTheId = null,
                SoTien = 500000
            };

            // Act
            var results = ValidateModel(model);

            // Assert
            Assert.Contains(results, r => r.ErrorMessage.Contains("Chọn phương thức thanh toán"));
            Assert.Contains(results, r => r.ErrorMessage.Contains("Chọn ngân hàng"));
            Assert.Contains(results, r => r.ErrorMessage.Contains("Nhập số tài khoản"));
            Assert.Contains(results, r => r.ErrorMessage.Contains("Nhập tên chủ tài khoản"));
            Assert.Contains(results, r => r.ErrorMessage.Contains("Vui lòng chọn thẻ hoặc ví."));
            Assert.Equal(5, results.Count);
        }

        [Fact]
        public void ThongTinThanhToan_ValidModel_ShouldPassValidation()
        {
            // Arrange
            var model = new ThongTinThanhToan
            {
                PhuongThuc = "Chuyển khoản",
                TenNganHang = "Vietcombank",
                SoTaiKhoan = "0123456789",
                ChuTaiKhoan = "Nguyen Van A",
                SelectedTheId = "the123",
                SoTien = 500000
            };

            // Act
            var results = ValidateModel(model);

            // Assert
            Assert.Empty(results);
        }
    }
}
