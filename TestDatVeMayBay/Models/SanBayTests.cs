using QLDatVeMayBay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

public class SanBayTests
{
    private IList<ValidationResult> ValidateModel(object model)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, context, results, true);
        return results;
    }

    [Fact]
    public void TenSanBay_KhongVuotQua100KyTu()
    {
        var sanBay = new SanBay
        {
            TenSanBay = new string('A', 101),
            DiaDiem = "Ha Noi"
        };

        var results = ValidateModel(sanBay);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(SanBay.TenSanBay)));
    }

    [Fact]
    public void DiaDiem_KhongVuotQua100KyTu()
    {
        var sanBay = new SanBay
        {
            TenSanBay = "Noi Bai",
            DiaDiem = new string('B', 101)
        };

        var results = ValidateModel(sanBay);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(SanBay.DiaDiem)));
    }

    [Fact]
    public void DuLieu_HopLe_KhongCoLoi()
    {
        var sanBay = new SanBay
        {
            TenSanBay = "Tan Son Nhat",
            DiaDiem = "TP HCM"
        };

        var results = ValidateModel(sanBay);

        Assert.Empty(results);
    }
}
