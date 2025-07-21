using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QLDatVeMayBay.Models.Entities;
using Xunit;

namespace QLDatVeMayBay.Tests
{
    public class TheThanhToanTests
    {
        private List<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            Validator.TryValidateObject(model, context, results, validateAllProperties: true);
            return results;
        }

        [Fact]
        public void TheThanhToan_ValidData_ShouldPassValidation()
        {
            var the = new TheThanhToan
            {
                NguoiDungId = 1,
                Loai = LoaiTheLoaiVi.TheNganHang,
                TenNganHang = "Vietcombank",
                SoThe = "1234567890123456",
                HieuLuc = "12/25",
                CVV = "123",
                TenTrenThe = "Nguyen Van A",
                TenHienThi = "Thẻ ngân hàng chính"
            };

            var results = ValidateModel(the);

            Assert.Empty(results);
        }

        
        [Fact]
        public void TheThanhToan_InvalidEmail_ShouldFailValidation()
        {
            var the = new TheThanhToan
            {
                NguoiDungId = 1,
                Loai = LoaiTheLoaiVi.ViDienTu,
                EmailLienKet = "khongphaiemail",
            };

            var results = ValidateModel(the);

            Assert.Contains(results, r => r.ErrorMessage.Contains("Email không hợp lệ"));
        }

        [Fact]
        public void TheThanhToan_InvalidPhone_ShouldFailValidation()
        {
            var the = new TheThanhToan
            {
                NguoiDungId = 1,
                Loai = LoaiTheLoaiVi.ViDienTu,
                SoDienThoai = "abcd123", // sai định dạng
            };

            var results = ValidateModel(the);

            Assert.Contains(results, r => r.ErrorMessage.Contains("Số điện thoại không hợp lệ"));
        }

        [Fact]
        public void TheThanhToan_SoTheQuaDai_ShouldFailValidation()
        {
            var the = new TheThanhToan
            {
                NguoiDungId = 1,
                Loai = LoaiTheLoaiVi.TheNganHang,
                SoThe = new string('1', 30), // quá 20 ký tự
            };

            var results = ValidateModel(the);

            Assert.Contains(results, r => r.MemberNames.Contains("SoThe"));
        }
    }
}
