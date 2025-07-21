using QLDatVeMayBay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace TestDatVeMayBay.Tests.Models
{
    public class LoaiMayBayTests
    {
        // Phương thức dùng để xác thực model
        private IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

       
       
    }
}
