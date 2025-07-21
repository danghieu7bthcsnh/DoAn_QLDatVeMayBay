using QLDatVeMayBay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace TestDatVeMayBay.Tests.Models
{
    public class QuenMatKhauTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void QuenMatKhau_Email_HopLe()
        {
            var model = new QuenMatKhau
            {
                Email = "abc@example.com"
            };

            var results = ValidateModel(model);

            Assert.Empty(results);
        }

        [Fact]
        public void QuenMatKhau_Email_Thieu()
        {
            var model = new QuenMatKhau
            {
                Email = ""
            };

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }

        [Fact]
        public void QuenMatKhau_Email_KhongHopLe()
        {
            var model = new QuenMatKhau
            {
                Email = "khonghople.com"
            };

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("Email"));
        }
    }
}
