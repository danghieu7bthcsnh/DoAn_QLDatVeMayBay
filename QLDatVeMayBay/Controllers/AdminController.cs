using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLDatVeMayBay.AdminViewModels;
using QLDatVeMayBay.Data;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace QLDatVeMayBay.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly QLDatVeMayBayContext _context;

        public AdminController(QLDatVeMayBayContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Dashboard()
        {
            var tenDangNhap = HttpContext.Session.GetString("TenDangNhap");
            if (string.IsNullOrEmpty(tenDangNhap))
                return RedirectToAction("DangNhap", "TaiKhoan");

            var taiKhoan = await _context.TaiKhoan.FindAsync(tenDangNhap);
            if (taiKhoan == null || taiKhoan.VaiTro != "Admin")
                return Unauthorized();

            // Tổng số người dùng
            ViewBag.TongNguoiDung = await _context.NguoiDung.CountAsync();

            // Tổng số chuyến bay
            ViewBag.TongChuyenBay = await _context.ChuyenBay.CountAsync();

            // Tổng số vé máy bay
            ViewBag.TongVe = await _context.VeMayBay.CountAsync();

            // Doanh thu tháng hiện tại (tính tổng số tiền ThanhToan trong tháng này)
            var now = DateTime.Now;
            ViewBag.DoanhThuThang = await _context.ThanhToan
                .Where(t => t.ThoiGianGiaoDich.Year == now.Year && t.ThoiGianGiaoDich.Month == now.Month)
                .SumAsync(t => (decimal?)t.SoTien) ?? 0m;

            // Lấy nhãn tháng và doanh thu thực tế theo từng tháng trong năm hiện tại
            var doanhThuTheoThang = await _context.ThanhToan
                .Where(t => t.ThoiGianGiaoDich.Year == now.Year)
                .GroupBy(t => t.ThoiGianGiaoDich.Month)
                .Select(g => new { Thang = g.Key, TongDoanhThu = g.Sum(x => x.SoTien) })
                .ToListAsync();

            // Chuẩn bị dữ liệu cho biểu đồ doanh thu tháng
            var thangLabels = Enumerable.Range(1, 12).Select(m => $"Tháng {m}").ToList();
            var doanhThuData = new decimal[12];
            foreach (var item in doanhThuTheoThang)
            {
                doanhThuData[item.Thang - 1] = item.TongDoanhThu;
            }
            ViewBag.ThangLabels = thangLabels;
            ViewBag.DoanhThuData = doanhThuData;

            // Số lượng vé theo trạng thái (đã thanh toán, chưa thanh toán, hủy)
            var veStatusData = await _context.VeMayBay
                .GroupBy(v => v.TrangThaiVe)
                .Select(g => new { TrangThai = g.Key, Count = g.Count() })
                .ToListAsync();

            var trangThaiLabels = new List<string> { "Đã thanh toán", "Chưa thanh toán", "Đã hủy" };
            var trangThaiCounts = new List<int>
    {
        veStatusData.FirstOrDefault(x => x.TrangThai == "Đã thanh toán")?.Count ?? 0,
        veStatusData.FirstOrDefault(x => x.TrangThai == "Chưa thanh toán")?.Count ?? 0,
        veStatusData.FirstOrDefault(x => x.TrangThai == "Đã hủy")?.Count ?? 0,
    };
            ViewBag.VeStatusLabels = trangThaiLabels;
            ViewBag.VeStatusData = trangThaiCounts;

            // Lấy 5 người dùng mới đăng ký gần nhất theo ngày tạo tài khoản (TaiKhoan.NgayTao)
            ViewBag.NguoiDungMoi = await _context.NguoiDung
                .Include(u => u.TaiKhoan)
                .OrderByDescending(u => u.TaiKhoan.NgayTao)
                .Take(5)
                .Select(u => new
                {
                    u.TenDangNhap,
                    u.HoTen,
                    u.Email,
                    NgayTao = u.TaiKhoan.NgayTao
                })
                .ToListAsync();

            // Vé đặt hôm nay
            var today = DateTime.Today;
            ViewBag.VeDatHomNay = await _context.VeMayBay.CountAsync(v => v.ThoiGianDat.Date == today);

            // Vé bị hủy trong tháng hiện tại
            ViewBag.VeHuyThang = await _context.VeMayBay
                .CountAsync(v => v.TrangThaiVe == "Đã hủy" && v.ThoiGianDat.Year == now.Year && v.ThoiGianDat.Month == now.Month);

            // Người dùng mới hôm nay
            ViewBag.NguoiDungMoiHomNay = await _context.NguoiDung
                .Include(u => u.TaiKhoan)
                .CountAsync(u => u.TaiKhoan.NgayTao.HasValue && u.TaiKhoan.NgayTao.Value.Date == today);

            return View();
        }
<<<<<<< Updated upstream
        
=======
        [HttpGet]
        public async Task<IActionResult> CaNhan()
        {
            var tenDangNhap = HttpContext.Session.GetString("TenDangNhap");
            if (string.IsNullOrEmpty(tenDangNhap))
                return RedirectToAction("DangNhap", "TaiKhoan");

            var nguoiDung = await _context.NguoiDung
                .Include(u => u.TaiKhoan)
                .FirstOrDefaultAsync(u => u.TenDangNhap == tenDangNhap);

            if (nguoiDung == null) return NotFound();

            var model = new ThongTinCaNhanViewModel
            {
                TenDangNhap = nguoiDung.TenDangNhap,
                HoTen = nguoiDung.HoTen,
                Email = nguoiDung.Email,
                SoDienThoai = nguoiDung.SoDienThoai,
                GioiTinh = nguoiDung.GioiTinh,
                QuocTich = nguoiDung.QuocTich,
                CCCD = nguoiDung.CCCD
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CaNhan(ThongTinCaNhanViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var tenDangNhap = HttpContext.Session.GetString("TenDangNhap");
            if (string.IsNullOrEmpty(tenDangNhap))
                return RedirectToAction("DangNhap", "TaiKhoan");

            var nguoiDung = await _context.NguoiDung.FirstOrDefaultAsync(u => u.TenDangNhap == tenDangNhap);
            if (nguoiDung == null) return NotFound();

            // Kiểm tra trùng email
            bool emailDaTonTai = await _context.NguoiDung.AnyAsync(u => u.Email == model.Email && u.TenDangNhap != tenDangNhap);
            if (emailDaTonTai)
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng bởi người dùng khác");
                return View(model);
            }

            // Kiểm tra trùng số điện thoại nếu có
            if (!string.IsNullOrEmpty(model.SoDienThoai))
            {
                bool sdtDaTonTai = await _context.NguoiDung.AnyAsync(u => u.SoDienThoai == model.SoDienThoai && u.TenDangNhap != tenDangNhap);
                if (sdtDaTonTai)
                {
                    ModelState.AddModelError("SoDienThoai", "Số điện thoại đã được sử dụng bởi người dùng khác");
                    return View(model);
                }
            }

            // Kiểm tra trùng CCCD nếu có
            if (!string.IsNullOrEmpty(model.CCCD))
            {
                bool cccdDaTonTai = await _context.NguoiDung.AnyAsync(u => u.CCCD == model.CCCD && u.TenDangNhap != tenDangNhap);
                if (cccdDaTonTai)
                {
                    ModelState.AddModelError("CCCD", "CCCD/CMND đã được sử dụng bởi người dùng khác");
                    return View(model);
                }
            }

            // Cập nhật thông tin
            nguoiDung.HoTen = model.HoTen.Trim();
            nguoiDung.Email = model.Email.Trim();
            nguoiDung.SoDienThoai = model.SoDienThoai?.Trim();
            nguoiDung.GioiTinh = model.GioiTinh;
            nguoiDung.QuocTich = model.QuocTich?.Trim();
            nguoiDung.CCCD = model.CCCD?.Trim();

            _context.NguoiDung.Update(nguoiDung);
            await _context.SaveChangesAsync();

            ViewBag.ThongBao = "Cập nhật thông tin cá nhân thành công!";
            return View(model);
        }


        // Đổi mật khẩu (GET)
        [HttpGet]
        public IActionResult DoiMatKhau()
        {
            return View();
        }

        // Đổi mật khẩu (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiMatKhau(DoiMatKhauViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var tenDangNhap = HttpContext.Session.GetString("TenDangNhap");
            if (string.IsNullOrEmpty(tenDangNhap))
                return RedirectToAction("DangNhap", "TaiKhoan");

            var taiKhoan = await _context.TaiKhoan.FindAsync(tenDangNhap);
            if (taiKhoan == null) return NotFound();

            // ✅ Kiểm tra mật khẩu cũ bằng BCrypt
            if (!BCrypt.Net.BCrypt.Verify(model.MatKhauCu, taiKhoan.MatKhau))
            {
                ModelState.AddModelError("MatKhauCu", "❌ Mật khẩu hiện tại không đúng");
                return View(model);
            }

            // ✅ Cập nhật mật khẩu mới đã mã hóa
            taiKhoan.MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhauMoi);
            _context.TaiKhoan.Update(taiKhoan);
            await _context.SaveChangesAsync();

            TempData["Success"] = "✅ Đổi mật khẩu thành công!";
            return RedirectToAction("DoiMatKhau");
        }

        [HttpGet]
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }


>>>>>>> Stashed changes
    }
}
