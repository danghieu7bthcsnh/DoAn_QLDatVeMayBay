using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLDatVeMayBay.Data;
using QLDatVeMayBay.Models.Entities;
using QLDatVeMayBay.Models.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QLDatVeMayBay.Controllers
{
    [Authorize]
    public class TheThanhToanController : Controller
    {
        private readonly QLDatVeMayBayContext _context;

        public TheThanhToanController(QLDatVeMayBayContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? showForm)
        {
            try
            {
                var tenDangNhap = User.Identity?.Name;
                var nguoiDung = await _context.NguoiDung.FirstOrDefaultAsync(u => u.TenDangNhap == tenDangNhap);

                if (nguoiDung == null)
                    return RedirectToAction("DangNhap", "TaiKhoan");

                var danhSach = await _context.TheThanhToan
                    .Where(t => t.NguoiDungId == nguoiDung.IDNguoiDung)
                    .ToListAsync();

                // ✅ Gán đúng kiểu nếu có showForm, ngược lại giữ null (ẩn form)
                LoaiTheLoaiVi? loai = null;
                if (showForm == "The")
                    loai = LoaiTheLoaiVi.TheNganHang;
                else if (showForm == "Vi")
                    loai = LoaiTheLoaiVi.ViDienTu;

                var vm = new TheThanhToanViewModel
                {
                    NguoiDungId = nguoiDung.IDNguoiDung,
                    Loai = loai, // ✅ null nếu không chọn gì, dùng để ẩn form
                    DanhSach = danhSach,
                    NgayLienKet = DateTime.Today
                };

                return View("~/Views/TheThanhToan/Index.cshtml", vm);
            }
            catch (Exception ex)
            {
                TempData["Debug"] = $"Lỗi Index: {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }



        [HttpPost]
        public async Task<IActionResult> CreateTheNganHang(string SoThe, string TenTrenThe, DateTime HieuLuc, string CVV)
        {
            var tenDangNhap = User.Identity?.Name;
            var nguoiDung = await _context.NguoiDung.FirstOrDefaultAsync(u => u.TenDangNhap == tenDangNhap);

            if (nguoiDung == null)
                return RedirectToAction("DangNhap", "TaiKhoan");

            var model = new TheThanhToan
            {
                Id = Guid.NewGuid().ToString(),
                NguoiDungId = nguoiDung.IDNguoiDung,
                Loai = LoaiTheLoaiVi.TheNganHang,
                SoThe = SoThe,
                TenTrenThe = TenTrenThe,
                HieuLuc = HieuLuc.ToString("MM/yyyy"),
                CVV = CVV,
                NgayLienKet = DateTime.Now
            };

            _context.Add(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Thêm thẻ ngân hàng thành công!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> CreateViDienTu(string TenVi, string EmailLienKet, string TenHienThi, string SoDienThoai, DateTime NgayLienKet)
        {
            var tenDangNhap = User.Identity?.Name;
            var nguoiDung = await _context.NguoiDung.FirstOrDefaultAsync(u => u.TenDangNhap == tenDangNhap);

            if (nguoiDung == null)
                return RedirectToAction("DangNhap", "TaiKhoan");

            // ✅ Nếu người dùng không chọn ngày, gán mặc định là hôm nay
            if (NgayLienKet == default(DateTime))
                NgayLienKet = DateTime.Today;

            var model = new TheThanhToan
            {
                Id = Guid.NewGuid().ToString(),
                NguoiDungId = nguoiDung.IDNguoiDung,
                Loai = LoaiTheLoaiVi.ViDienTu,
                TenVi = TenVi,
                EmailLienKet = EmailLienKet,
                TenHienThi = TenHienThi,
                SoDienThoai = SoDienThoai,
                NgayLienKet = NgayLienKet
            };

            _context.Add(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Thêm ví điện tử thành công!";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var model = await _context.TheThanhToan.FindAsync(id);
            if (model == null) return NotFound();

            var vm = new TheThanhToanViewModel
            {
                Id = model.Id,
                NguoiDungId = model.NguoiDungId,
                Loai = model.Loai,
                SoThe = model.SoThe,
                HieuLuc = model.HieuLuc,
                CVV = model.CVV,
                TenTrenThe = model.TenTrenThe,
                TenVi = model.TenVi,
                EmailLienKet = model.EmailLienKet,
                TenHienThi = model.TenHienThi,
                SoDienThoai = model.SoDienThoai,
                NgayLienKet = model.NgayLienKet
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TheThanhToanViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Debug"] = "Dữ liệu không hợp lệ";
                return View(vm);
            }

            var entity = await _context.TheThanhToan.FindAsync(vm.Id);
            if (entity == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin
            entity.Loai = vm.Loai.Value;
            entity.SoThe = vm.SoThe;
            entity.HieuLuc = vm.HieuLuc;
            entity.CVV = vm.CVV;
            entity.TenTrenThe = vm.TenTrenThe;
            entity.TenVi = vm.TenVi;
            entity.EmailLienKet = vm.EmailLienKet;
            entity.TenHienThi = vm.TenHienThi;
            entity.SoDienThoai = vm.SoDienThoai;
            entity.NgayLienKet = vm.NgayLienKet;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật thành công!";
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var model = await _context.TheThanhToan.FindAsync(id);
            if (model == null) return NotFound();

            _context.TheThanhToan.Remove(model);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Đã xoá thành công";
            return RedirectToAction(nameof(Index));
        }
    }
}
