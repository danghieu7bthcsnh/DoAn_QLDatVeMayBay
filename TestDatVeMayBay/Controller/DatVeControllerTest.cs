using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QLDatVeMayBay.Controllers;
using QLDatVeMayBay.Data;
using QLDatVeMayBay.Models;
using QLDatVeMayBay.Models.Entities;
using QLDatVeMayBay.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DatVeControllerTest
{
    private readonly DatVeController _controller;
    private readonly QLDatVeMayBayContext _context;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly Mock<EmailService> _mockEmailService;

    public DatVeControllerTest()
    {
        var options = new DbContextOptionsBuilder<QLDatVeMayBayContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new QLDatVeMayBayContext(options);
        _mockConfig = new Mock<IConfiguration>();
        _mockEmailService = new Mock<EmailService>(_mockConfig.Object);
        _controller = new DatVeController(_context, _mockConfig.Object, _mockEmailService.Object);
    }

    private ISession CreateMockSession(Dictionary<string, string> keyValues)
    {
        var session = new Mock<ISession>();
        foreach (var kvp in keyValues)
        {
            session.Setup(s => s.TryGetValue(kvp.Key, out It.Ref<byte[]>.IsAny))
                .Callback<string, byte[]>((key, val) => val = Encoding.UTF8.GetBytes(kvp.Value))
                .Returns(true);

            session.Setup(s => s.GetString(kvp.Key)).Returns(kvp.Value);
        }

        return session.Object;
    }

    [Fact]
    public async Task ChonGhe_Returns_View_With_Correct_Model()
    {
        var loaiMayBay = new LoaiMayBay { LoaiMayBayId = 1, TongSoGhe = 150 };
        var mayBay = new MayBay { IDMayBay = 1, LoaiMayBay = loaiMayBay };
        var chuyenBay = new ChuyenBay { IDChuyenBay = 1, MayBay = mayBay };
        _context.LoaiMayBay.Add(loaiMayBay);
        _context.MayBay.Add(mayBay);
        _context.ChuyenBay.Add(chuyenBay);
        await _context.SaveChangesAsync();

        var result = await _controller.ChonGhe(1);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<DatGhe>(viewResult.Model);
        Assert.Equal(150, model.TongSoGhe);
    }

   
}
