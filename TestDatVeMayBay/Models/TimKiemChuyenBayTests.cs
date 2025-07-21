using QLDatVeMayBay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace TestDatVeMayBay.Tests.Models
{
    public class TimKiemChuyenBayTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void TimKiemChuyenBay_HopLe_KhongCoLoi()
        {
            var timKiem = new TimKiemChuyenBay
            {
                SanBayDi = 1,
                SanBayDen = 2,
                NgayDi = DateTime.Today.AddDays(1),
                LoaiVe = "MotChieu",
                HangGhe = "PhoThong",
                NguoiLon = 1,
                TreEm = 0,
                EmBe = 0
            };

            var results = ValidateModel(timKiem);
            Assert.Empty(results); // Không có lỗi
        }


       

        [Fact]
        public void TimKiemChuyenBay_SoNguoiLonMacDinh_KhongBaoLoi()
        {
            var timKiem = new TimKiemChuyenBay
            {
                SanBayDi = 1,
                SanBayDen = 2,
                NgayDi = DateTime.Today,
                LoaiVe = "MotChieu",
                HangGhe = "PhoThong",
                NguoiLon = 0 // hợp lệ, không bắt buộc > 0
            };

            var results = ValidateModel(timKiem);
            Assert.Empty(results);
        }
    }
}
