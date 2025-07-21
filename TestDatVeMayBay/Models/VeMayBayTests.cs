using QLDatVeMayBay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace TestDatVeMayBay.Tests.Models
{
    public class VeMayBayTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void VeMayBay_HopLe_KhongCoLoi()
        {
            var ve = new VeMayBay
            {
                IDVe = 1,
                IDNguoiDung = 10,
                IDChuyenBay = 20,
                IDGhe = 30,
                ThoiGianDat = DateTime.Now,
                TrangThaiVe = "Chưa thanh toán",
                HangGhe = "PhoThong",
                LoaiVe = "MotChieu",
                TheThanhToanId = "abc123"
            };

            var results = ValidateModel(ve);
            Assert.Empty(results); // Không có lỗi
        }

        [Fact]
        public void VeMayBay_TrangThaiVuotQuaDoDai_BaoLoi()
        {
            var ve = new VeMayBay
            {
                IDVe = 1,
                IDNguoiDung = 10,
                IDChuyenBay = 20,
                IDGhe = 30,
                ThoiGianDat = DateTime.Now,
                TrangThaiVe = new string('a', 51), // quá 50 ký tự
                TheThanhToanId = "abc123"
            };

            var results = ValidateModel(ve);
            Assert.Contains(results, r => r.MemberNames.Contains("TrangThaiVe"));
        }

        [Fact]
        public void VeMayBay_TheThanhToanIdVuotQua450_BaoLoi()
        {
            var ve = new VeMayBay
            {
                IDVe = 1,
                IDNguoiDung = 10,
                IDChuyenBay = 20,
                IDGhe = 30,
                ThoiGianDat = DateTime.Now,
                TheThanhToanId = new string('x', 451)
            };

            var results = ValidateModel(ve);
            Assert.Contains(results, r => r.MemberNames.Contains("TheThanhToanId"));
        }

        [Fact]
        public void VeMayBay_TheThanhToanIdRong_KhongBaoLoi()
        {
            var ve = new VeMayBay
            {
                IDVe = 1,
                IDNguoiDung = 10,
                IDChuyenBay = 20,
                IDGhe = 30,
                ThoiGianDat = DateTime.Now
                // TheThanhToanId không bắt buộc => hợp lệ
            };

            var results = ValidateModel(ve);
            Assert.Empty(results);
        }
    }
}
