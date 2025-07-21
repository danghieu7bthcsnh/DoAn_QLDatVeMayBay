using QLDatVeMayBay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace TestDatVeMayBay.Tests.Models
{
    public class MaXacNhanTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void MaXacNhan_HopLe_KhongCoLoi()
        {
            var model = new MaXacNhan
            {
                TenDangNhap = "huyendo123",
                Ma = "ABC123",
                ThoiGianHetHan = DateTime.Now.AddMinutes(15)
            };

            var results = ValidateModel(model);
            Assert.Empty(results);
        }

        [Fact]
        public void MaXacNhan_ThieuTenDangNhap()
        {
            var model = new MaXacNhan
            {
                TenDangNhap = null!,
                Ma = "ABC123",
                ThoiGianHetHan = DateTime.Now.AddMinutes(10)
            };

            var results = ValidateModel(model);
            Assert.Contains(results, r => r.MemberNames.Contains("TenDangNhap"));
        }

        [Fact]
        public void MaXacNhan_ThieuMa()
        {
            var model = new MaXacNhan
            {
                TenDangNhap = "user456",
                Ma = null!,
                ThoiGianHetHan = DateTime.Now.AddMinutes(10)
            };

            var results = ValidateModel(model);
            Assert.Contains(results, r => r.MemberNames.Contains("Ma"));
        }

        [Fact]
        public void MaXacNhan_ThoiGianHetHan_TuongLai()
        {
            var model = new MaXacNhan
            {
                TenDangNhap = "user456",
                Ma = "M123",
                ThoiGianHetHan = DateTime.Now.AddHours(1)
            };

            var results = ValidateModel(model);
            Assert.Empty(results); // không có xác thực thời gian nên không lỗi
        }
    }
}
