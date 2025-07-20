using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLDatVeMayBay.Data;
using QLDatVeMayBay.ViewModels;
using System;

namespace QLDatVeMayBay.Controllers
{
    public class GiaoDichController : Controller
    {
        private readonly QLDatVeMayBayContext _context;

        public GiaoDichController(QLDatVeMayBayContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var giaoDichList = _context.ThanhToan
                .Include(t => t.VeMayBay)
                    .ThenInclude(v => v.NguoiDung)
                .Select(t => new GiaoDichViewModel
                {
                    IDThanhToan = t.IDThanhToan,
                    IDVe = t.IDVe,
                    HoTenNguoiDung = t.VeMayBay!.NguoiDung!.HoTen,
                    TenDangNhap = t.VeMayBay!.NguoiDung!.TenDangNhap,
                    SoTien = t.SoTien,
                    PhuongThuc = t.PhuongThuc ?? "Không rõ",
                    ThoiGianGiaoDich = t.ThoiGianGiaoDich,
                    TrangThaiThanhToan = t.TrangThaiThanhToan ?? "Không rõ"
                })
                .OrderByDescending(t => t.ThoiGianGiaoDich)
                .ToList();

            return View(giaoDichList);
        }
    }
}
