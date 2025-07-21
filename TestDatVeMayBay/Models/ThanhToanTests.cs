using QLDatVeMayBay.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

public class ThanhToanTests
{
    private IList<ValidationResult> ValidateModel(object model)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, context, results, true);
        return results;
    }

    [Fact]
    public void PhuongThuc_VuotQuaDoDai_50KyTu_ThiBaoLoi()
    {
        var tt = new ThanhToan
        {
            IDVe = 1,
            SoTien = 100000,
            PhuongThuc = new string('A', 51),
            ThoiGianGiaoDich = DateTime.Now,
            TrangThaiThanhToan = "DaThanhToan"
        };

        var results = ValidateModel(tt);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(ThanhToan.PhuongThuc)));
    }

    [Fact]
    public void TrangThaiThanhToan_VuotQua50KyTu_ThiBaoLoi()
    {
        var tt = new ThanhToan
        {
            IDVe = 1,
            SoTien = 100000,
            PhuongThuc = "ChuyenKhoan",
            ThoiGianGiaoDich = DateTime.Now,
            TrangThaiThanhToan = new string('B', 51)
        };

        var results = ValidateModel(tt);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(ThanhToan.TrangThaiThanhToan)));
    }

    [Fact]
    public void DuLieuHopLe_KhongCoLoi()
    {
        var tt = new ThanhToan
        {
            IDVe = 1,
            SoTien = 200000,
            PhuongThuc = "TienMat",
            ThoiGianGiaoDich = DateTime.Now,
            TrangThaiThanhToan = "ThanhCong"
        };

        var results = ValidateModel(tt);

        Assert.Empty(results);
    }

    [Fact]
    public void SoTien_LaSoAm_VanKhongBaoLoi_DoChuaCoAttribute()
    {
        var tt = new ThanhToan
        {
            IDVe = 1,
            SoTien = -50000, // Không có [Range] nên không vi phạm validation
            PhuongThuc = "TienMat",
            ThoiGianGiaoDich = DateTime.Now,
            TrangThaiThanhToan = "ThatBai"
        };

        var results = ValidateModel(tt);

        Assert.Empty(results); // Chấp nhận giá trị âm vì không có ràng buộc
    }
}
