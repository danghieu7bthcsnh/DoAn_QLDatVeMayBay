using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLDatVeMayBay.Controllers;
using QLDatVeMayBay.Data;
using QLDatVeMayBay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestDatVeMayBay.Tests.Controller
{
    public class ChuyenBayControllerTest
    {
        private QLDatVeMayBayContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<QLDatVeMayBayContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // database riêng cho từng test
                .Options;

            var context = new QLDatVeMayBayContext(options);

            // Dữ liệu mẫu
            var sanBay1 = new SanBay { IDSanBay = 1, TenSanBay = "Nội Bài" };
            var sanBay2 = new SanBay { IDSanBay = 2, TenSanBay = "Tân Sơn Nhất" };

            context.SanBay.AddRange(sanBay1, sanBay2);

            context.ChuyenBay.Add(new ChuyenBay
            {
                IDChuyenBay = 1,
                SanBayDi = 1,
                SanBayDen = 2,
                GioCatCanh = DateTime.Today.AddDays(1),
                MayBay = new MayBay { IDMayBay = 1, TenHangHK = "VietNamAirline", LoaiMayBay = new LoaiMayBay { TongSoGhe = 180 } },
                SanBayDiInfo = sanBay1,
                SanBayDenInfo = sanBay2
            });

            context.SaveChanges();
            return context;
        }

        [Fact]
        public void TimKiem_GET_ReturnsViewWithSanBay()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new ChuyenBayController(context);

            // Act
            var result = controller.TimKiem() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ViewData.ContainsKey("SanBayDi"));
            Assert.True(result.ViewData.ContainsKey("SanBayDen"));
        }


        [Fact]
        public void TimKiem_POST_SameAirports_ReturnsError()
        {
            var context = GetDbContext();
            var controller = new ChuyenBayController(context);

            var model = new TimKiemChuyenBay
            {
                SanBayDi = 1,
                SanBayDen = 1,
                NgayDi = DateTime.Today.AddDays(1)
            };

            var result = controller.TimKiem(model) as ViewResult;

            Assert.Equal("Không tìm thấy chuyến bay phù hợp", result.ViewData["Loi"]);
        }

        [Fact]
        public void TimKiem_POST_PastDate_ReturnsError()
        {
            var context = GetDbContext();
            var controller = new ChuyenBayController(context);

            var model = new TimKiemChuyenBay
            {
                SanBayDi = 1,
                SanBayDen = 2,
                NgayDi = DateTime.Today.AddDays(-1)
            };

            var result = controller.TimKiem(model) as ViewResult;

            Assert.Equal("Ngày đi không hợp lệ", result.ViewData["Loi"]);
        }

        [Fact]
        public void TimKiem_POST_ValidInput_ReturnsResult()
        {
            var context = GetDbContext();
            var controller = new ChuyenBayController(context);

            var model = new TimKiemChuyenBay
            {
                SanBayDi = 1,
                SanBayDen = 2,
                NgayDi = DateTime.Today.AddDays(1)
            };

            var result = controller.TimKiem(model) as ViewResult;

            Assert.NotNull(result);
            Assert.True(result.ViewData.ContainsKey("KetQua"));
        }

        [Fact]
        public void KetQuaTimKiem_POST_ReturnsMatchingFlights()
        {
            var context = GetDbContext();
            var controller = new ChuyenBayController(context);

            var model = new TimKiemChuyenBay
            {
                SanBayDi = 1,
                SanBayDen = 2,
                NgayDi = DateTime.Today.AddDays(1)
            };

            var result = controller.KetQuaTimKiem(model) as ViewResult;

            Assert.NotNull(result);
            var flights = result.Model as List<ChuyenBay>;
            Assert.Single(flights);
        }
    }
}
