//using QLDatVeMayBay.Attributes;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using Xunit;

//public class RequiredIfAttributeTests
//{
//    private IList<ValidationResult> ValidateModel(object model)
//    {
//        var results = new List<ValidationResult>();
//        var context = new ValidationContext(model, null, null);
//        Validator.TryValidateObject(model, context, results, true);
//        return results;
//    }

//    [Fact]
//    public void DiaChiEmail_BatBuocKhi_LoaiXacNhanLaEmail()
//    {
//        var model = new TestModel
//        {
//            LoaiXacNhan = "Email",
//            DiaChiEmail = "" // Không nhập
//        };

//        var results = ValidateModel(model);

//        Assert.Contains(results, r => r.ErrorMessage == "Trường này bắt buộc khi xác nhận qua Email");
//    }

//    [Fact]
//    public void DiaChiEmail_KhongBatBuocKhi_LoaiXacNhanKhac()
//    {
//        var model = new TestModel
//        {
//            LoaiXacNhan = "SMS",
//            DiaChiEmail = null
//        };

//        var results = ValidateModel(model);

//        Assert.Empty(results);
//    }

//    [Fact]
//    public void DiaChiEmail_HopLeKhi_LoaiXacNhanLaEmailVaCoGiaTri()
//    {
//        var model = new TestModel
//        {
//            LoaiXacNhan = "Email",
//            DiaChiEmail = "abc@example.com"
//        };

//        var results = ValidateModel(model);

//        Assert.Empty(results);
//    }
//}
