using QLDatVeMayBay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace TestDatVeMayBay.Tests.Models
{
    public class HoanTienTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void HoanTien_HopLe_ValidationPass()
        {
            var ht = new HoanTien
            {
                IDHoanTien = 1,
                IDThanhToan = 1001,
                SoTienHoan = 150000m,
                LyDo = "Khách hủy chuyến"
            };

            var results = ValidateModel(ht);
            Assert.Empty(results);
        }

        
       

        [Fact]
        public void HoanTien_LyDoQuaDai_BaoLoiStringLength()
        {
            var ht = new HoanTien
            {
                IDHoanTien = 4,
                IDThanhToan = 1003,
                SoTienHoan = 120000m,
                LyDo = new string('L', 300) // dài hơn 200 ký tự
            };

            var results = ValidateModel(ht);
            Assert.Contains(results, r => r.MemberNames.Contains("LyDo"));
        }

        [Fact]
        public void HoanTien_NgayHoanTien_SetMacDinh()
        {
            var ht = new HoanTien
            {
                IDHoanTien = 5,
                IDThanhToan = 1004,
                SoTienHoan = 80000m
            };

            Assert.True(ht.NgayHoanTien.Date == DateTime.Now.Date); // chỉ so sánh ngày
        }
    }
}
