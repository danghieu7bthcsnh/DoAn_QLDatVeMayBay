using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLDatVeMayBay.Data;
using QLDatVeMayBay.Models;
using QLDatVeMayBay.ViewModels;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

namespace QLDatVeMayBay.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly QLDatVeMayBayContext _context;
        private readonly IConfiguration _configuration;

        public TaiKhoanController(QLDatVeMayBayContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: DangKy
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }

        // POST: DangKy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKy(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Kiểm tra trùng tên đăng nhập và email
            if (await _context.TaiKhoan.AnyAsync(t => t.TenDangNhap == model.TenDangNhap))
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại.");

            if (await _context.NguoiDung.AnyAsync(n => n.Email == model.Email))
                ModelState.AddModelError("Email", "Email đã được sử dụng.");

            if (!ModelState.IsValid)
                return View(model);

            // Mã hóa mật khẩu
            var matKhauHash = HashPassword(model.MatKhau);

            // Tạo tài khoản
            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = model.TenDangNhap,
                MatKhau = matKhauHash,
                VaiTro = "KhachHang",
                TrangThaiTK = "ChuaKichHoat"
            };

            var nguoiDung = new NguoiDung
            {
                TenDangNhap = model.TenDangNhap,
                HoTen = model.HoTen,
                Email = model.Email,
                SoDienThoai = model.SoDienThoai,
                GioiTinh = model.GioiTinh
            };

            // Thêm vào DB
            _context.Add(taiKhoan);
            _context.Add(nguoiDung);
            await _context.SaveChangesAsync();

            // Tạo mã xác nhận
            var ma = new Random().Next(100000, 999999).ToString();
            var maXacNhan = new MaXacNhan
            {
                TenDangNhap = model.TenDangNhap,
                Ma = ma,
                ThoiGianHetHan = DateTime.Now.AddMinutes(15)
            };
            _context.MaXacNhan.Add(maXacNhan);
            await _context.SaveChangesAsync();

            // Gửi email
            await SendEmailAsync(model.Email, "Xác nhận đăng ký", $"Mã xác nhận của bạn là: {ma}");

            TempData["TenDangNhap"] = model.TenDangNhap;
            TempData.Keep("TenDangNhap"); // giữ TempData qua redirect
            return RedirectToAction("XacNhanEmail");
        }

        // GET: XacNhanEmail
        [HttpGet]
        public IActionResult XacNhanEmail()
        {
            var tenDangNhap = TempData["TenDangNhap"]?.ToString();
            if (tenDangNhap == null) return RedirectToAction("DangKy");

            TempData.Keep("TenDangNhap"); // giữ TempData nếu reload trang
            return View(new XacNhanEmailViewModel { TenDangNhap = tenDangNhap });
        }

        // POST: XacNhanEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XacNhanEmail(XacNhanEmailViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var ma = await _context.MaXacNhan
                .FirstOrDefaultAsync(m => m.TenDangNhap == model.TenDangNhap && m.Ma == model.MaXacNhan);

            if (ma == null || ma.ThoiGianHetHan < DateTime.Now)
            {
                ModelState.AddModelError("MaXacNhan", "Mã xác nhận không đúng hoặc đã hết hạn.");
                return View(model);
            }

            var taiKhoan = await _context.TaiKhoan.FindAsync(model.TenDangNhap);
            if (taiKhoan != null)
            {
                taiKhoan.TrangThaiTK = "HoatDong";
                _context.MaXacNhan.Remove(ma);
                await _context.SaveChangesAsync();
            }

            TempData["XacNhanThanhCong"] = true;
            return RedirectToAction("DangNhap");
        }

        // GET: DangNhap
        [HttpGet]
        public IActionResult DangNhap()
        {
            return View();
        }

        // POST: DangNhap
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangNhap(DangNhapViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var taiKhoan = await _context.TaiKhoan
                .Include(t => t.NguoiDung)
                .FirstOrDefaultAsync(t => t.TenDangNhap == model.TenDangNhapOrEmail ||
                                          t.NguoiDung.Email == model.TenDangNhapOrEmail);

            if (taiKhoan == null || taiKhoan.TrangThaiTK != "HoatDong")
            {
                ModelState.AddModelError("TenDangNhapOrEmail", "Tài khoản không tồn tại hoặc chưa được kích hoạt.");
                return View(model);
            }

            var matKhauHash = HashPassword(model.MatKhau);
            if (taiKhoan.MatKhau != matKhauHash)
            {
                ModelState.AddModelError("MatKhau", "Mật khẩu không đúng.");
                return View(model);
            }

            // Đăng nhập thành công - tạo cookie xác thực với hoặc không nhớ đăng nhập
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, taiKhoan.TenDangNhap),
                new Claim(ClaimTypes.Role, taiKhoan.VaiTro)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.GhiNhoDangNhap, // nếu true thì cookie tồn tại lâu
                ExpiresUtc = model.GhiNhoDangNhap ? DateTimeOffset.UtcNow.AddDays(30) : (DateTimeOffset?)null
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        // Đăng xuất
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("DangNhap");
        }

        // Gửi email xác nhận
        private async Task SendEmailAsync(string email, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email người nhận không hợp lệ.");

            var emailMessage = new MimeMessage();

            var senderName = _configuration["EmailSettings:SenderName"];
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];

            emailMessage.From.Add(new MailboxAddress(senderName, senderEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(username, password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý lỗi gửi email
                throw new InvalidOperationException("Gửi email thất bại: " + ex.Message, ex);
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
