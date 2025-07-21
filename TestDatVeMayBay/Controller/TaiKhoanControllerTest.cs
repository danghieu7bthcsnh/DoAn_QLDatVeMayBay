using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using QLDatVeMayBay.Controllers;
using QLDatVeMayBay.Models;
using QLDatVeMayBay.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;
using QLDatVeMayBay.Services;
using QLDatVeMayBay.Data;
using Microsoft.Extensions.Configuration; // Giả định bạn có IEmailService


public class TaiKhoanControllerTests
{
    private readonly IConfiguration _configuration;

   

    private QLDatVeMayBayContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<QLDatVeMayBayContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new QLDatVeMayBayContext(options);
    }

    private RegisterViewModel GetValidRegisterViewModel()
    {
        return new RegisterViewModel
        {
            TenDangNhap = "newuser",
            MatKhau = "123456",
            Email = "newuser@example.com",
            HoTen = "Nguyen Van A",
            CCCD = "123456789",
            GioiTinh = "Nam",
            QuocTich = "VN",
            SoDienThoai = "0123456789"
        };
    }
    

    [Fact]
    public async Task DangKy_Post_TenDangNhapTrung_TraVeViewVoiLoi()
    {
        // Arrange
        var context = GetDbContext();
        context.TaiKhoan.Add(new TaiKhoan { TenDangNhap = "existinguser", MatKhau = "abc" });
        await context.SaveChangesAsync();
      
        var configMock = new Mock<IConfiguration>();
        var emailServiceMock = new Mock<EmailService>(configMock.Object);

        var controller = new TaiKhoanController(context, configMock.Object, emailServiceMock.Object);
        var model = GetValidRegisterViewModel();
        model.TenDangNhap = "existinguser";

        // Act
        var result = await controller.DangKy(model);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(viewResult.ViewData.ModelState.IsValid);
        Assert.True(viewResult.ViewData.ModelState.ContainsKey("TenDangNhap"));
    }

    [Fact]
    public async Task DangKy_Post_EmailTrung_TraVeViewVoiLoi()
    {
        // Arrange
        var context = GetDbContext(); // hàm tạo DbContext in-memory
        context.NguoiDung.Add(new NguoiDung { TenDangNhap = "abc", Email = "duplicate@example.com" });
        await context.SaveChangesAsync();

       
        var configMock = new Mock<IConfiguration>();
        var emailServiceMock = new Mock<EmailService>(configMock.Object);

        var controller = new TaiKhoanController(context, configMock.Object, emailServiceMock.Object);
       

        var model = GetValidRegisterViewModel();
        model.Email = "duplicate@example.com";

        // Act
        var result = await controller.DangKy(model);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(viewResult.ViewData.ModelState.IsValid);
        Assert.True(viewResult.ViewData.ModelState.ContainsKey("Email"));
    }

    [Fact]
    public async Task DangKy_Post_ModelStateKhongHopLe_TraVeView()
    {
        var context = GetDbContext(); // hàm tạo DbContext in-memory
        var configMock = new Mock<IConfiguration>();
        var emailServiceMock = new Mock<EmailService>(configMock.Object);

        var controller = new TaiKhoanController(context, configMock.Object, emailServiceMock.Object);

        controller.ModelState.AddModelError("MatKhau", "Mật khẩu là bắt buộc");

        var model = GetValidRegisterViewModel();
        model.MatKhau = "";

        // Act
        var result = await controller.DangKy(model);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(viewResult.ViewData.ModelState.IsValid);
    }
}
