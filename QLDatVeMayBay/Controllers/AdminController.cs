using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLDatVeMayBay.Data;

namespace QLDatVeMayBay.Controllers
{
    public class AdminController : Controller
    {
        private readonly QLDatVeMayBayContext _context;

        public AdminController(QLDatVeMayBayContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dashboard()
        {
            var tenDangNhap = HttpContext.Session.GetString("TenDangNhap");
            if (string.IsNullOrEmpty(tenDangNhap))
                return RedirectToAction("DangNhap", "TaiKhoan");

            var taiKhoan = await _context.TaiKhoan.FindAsync(tenDangNhap);
            if (taiKhoan == null || taiKhoan.VaiTro != "Admin")
                return Unauthorized();

            ViewBag.TongNguoiDung = await _context.NguoiDung.CountAsync();
            ViewBag.TongChuyenBay = await _context.ChuyenBay.CountAsync();
            ViewBag.TongVe = await _context.VeMayBay.CountAsync();

            return View();
        }

    }
}