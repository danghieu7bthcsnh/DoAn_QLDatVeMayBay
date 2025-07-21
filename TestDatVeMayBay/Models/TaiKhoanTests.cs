using QLDatVeMayBay.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

public class TaiKhoanTests
{
    private IList<ValidationResult> ValidateModel(object model)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, context, results, true);
        return results;
    }

    [Fact]
    public void TenDangNhap_Rong_ThiBaoLoi()
    {
        var taiKhoan = new TaiKhoan
        {
            TenDangNhap = "",
            MatKhau = "123456"
        };

        var results = ValidateModel(taiKhoan);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(TaiKhoan.TenDangNhap)));
    }

    [Fact]
    public void MatKhau_Rong_ThiBaoLoi()
    {
        var taiKhoan = new TaiKhoan
        {
            TenDangNhap = "user1",
            MatKhau = ""
        };

        var results = ValidateModel(taiKhoan);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(TaiKhoan.MatKhau)));
    }

    [Fact]
    public void VaiTro_KhongVuotQua20KyTu()
    {
        var taiKhoan = new TaiKhoan
        {
            TenDangNhap = "user2",
            MatKhau = "123456",
            VaiTro = new string('A', 21)
        };

        var results = ValidateModel(taiKhoan);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(TaiKhoan.VaiTro)));
    }

    [Fact]
    public void TrangThaiTK_KhongVuotQua20KyTu()
    {
        var taiKhoan = new TaiKhoan
        {
            TenDangNhap = "user3",
            MatKhau = "123456",
            TrangThaiTK = new string('B', 21)
        };

        var results = ValidateModel(taiKhoan);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(TaiKhoan.TrangThaiTK)));
    }

    [Fact]
    public void DuLieu_HopLe_KhongCoLoi()
    {
        var taiKhoan = new TaiKhoan
        {
            TenDangNhap = "user4",
            MatKhau = "matkhau123",
            VaiTro = "Admin",
            TrangThaiTK = "DaKichHoat"
        };

        var results = ValidateModel(taiKhoan);

        Assert.Empty(results);
        Assert.Equal(0, taiKhoan.SoLanDangNhapSai);
        Assert.NotNull(taiKhoan.NgayTao);
    }
}
