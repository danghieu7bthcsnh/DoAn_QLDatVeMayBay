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
using System.Net.Mail;
using System.Net;

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
            var nguoiDung = await _context.NguoiDung.FirstOrDefaultAsync(n => n.TenDangNhap == taiKhoan.TenDangNhap);
            if (nguoiDung != null)
            {
                HttpContext.Session.SetInt32("IDNguoiDung", nguoiDung.IDNguoiDung); // ✅ THÊM DÒNG NÀY
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

            HttpContext.Session.SetString("TenDangNhap", taiKhoan.TenDangNhap);
            HttpContext.Session.SetString("VaiTro", taiKhoan.VaiTro);


            return RedirectToAction("TimKiem", "ChuyenBay");
        }

        // Đăng xuất
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("DangNhap");
        }

        private async Task SendEmailAsync(string emailNguoiNhan, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(emailNguoiNhan))
                throw new ArgumentException("Email người nhận không hợp lệ.");

            var emailMessage = new MimeMessage();

            var senderName = _configuration["EmailSettings:SenderName"];
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];

            emailMessage.From.Add(new MailboxAddress(senderName, senderEmail));
            emailMessage.To.Add(new MailboxAddress("", emailNguoiNhan)); // <-- email người dùng
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(username, password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Gửi email thất bại: " + ex.Message, ex);
            }
        }
        [HttpGet]
        public IActionResult QuenMatKhau()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuenMatKhau(QuenMatKhauViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var nguoiDung = await _context.NguoiDung.FirstOrDefaultAsync(n => n.Email == model.Email);
            if (nguoiDung == null)
            {
                ModelState.AddModelError("Email", "Email này chưa được đăng ký.");
                return View(model);
            }

            var ma = new Random().Next(100000, 999999).ToString();
            var maXacNhan = new MaXacNhan
            {
                TenDangNhap = nguoiDung.TenDangNhap,
                Ma = ma,
                ThoiGianHetHan = DateTime.Now.AddMinutes(10)
            };

            _context.MaXacNhan.Add(maXacNhan);
            await _context.SaveChangesAsync();

            await SendEmailAsync(model.Email, "Xác nhận quên mật khẩu", $"Mã xác nhận của bạn là: {ma}");

            TempData["Email"] = model.Email;
            return RedirectToAction("XacNhanQuenMatKhau");
        }

        [HttpGet]
        public IActionResult XacNhanQuenMatKhau()
        {
            var email = TempData["Email"]?.ToString();
            if (string.IsNullOrEmpty(email)) return RedirectToAction("QuenMatKhau");

            TempData.Keep("Email");
            return View(new XacNhanQuenMatKhauViewModel { Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XacNhanQuenMatKhau(XacNhanQuenMatKhauViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var nguoiDung = await _context.NguoiDung.FirstOrDefaultAsync(n => n.Email == model.Email);
            var ma = await _context.MaXacNhan.FirstOrDefaultAsync(m =>
                m.TenDangNhap == nguoiDung.TenDangNhap &&
                m.Ma == model.MaXacNhan &&
                m.ThoiGianHetHan >= DateTime.Now);

            if (ma == null)
            {
                ModelState.AddModelError("MaXacNhan", "Mã không chính xác hoặc đã hết hạn.");
                return View(model);
            }

            TempData["Email"] = model.Email;
            return RedirectToAction("DoiMatKhau");
        }

        [HttpGet]
        public IActionResult DoiMatKhau()
        {
            var email = TempData["Email"]?.ToString();
            if (string.IsNullOrEmpty(email)) return RedirectToAction("QuenMatKhau");

            TempData.Keep("Email");
            return View(new DoiMatKhauViewModel { Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoiMatKhau(DoiMatKhauViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var nguoiDung = await _context.NguoiDung.FirstOrDefaultAsync(n => n.Email == model.Email);
            var taiKhoan = await _context.TaiKhoan.FirstOrDefaultAsync(t => t.TenDangNhap == nguoiDung.TenDangNhap);

            if (taiKhoan == null) return RedirectToAction("QuenMatKhau");

            taiKhoan.MatKhau = HashPassword(model.MatKhauMoi);
            var maXacNhanList = _context.MaXacNhan.Where(m => m.TenDangNhap == taiKhoan.TenDangNhap);
            _context.MaXacNhan.RemoveRange(maXacNhanList);

            await _context.SaveChangesAsync();
            TempData["ThongBao"] = "Đổi mật khẩu thành công. Hãy đăng nhập lại.";
            return RedirectToAction("DangNhap");
        }


        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        [HttpGet]
        public async Task<IActionResult> CaNhan()
        {
            var tenDangNhap = HttpContext.Session.GetString("TenDangNhap");
            if (string.IsNullOrEmpty(tenDangNhap))
                return RedirectToAction("DangNhap", "TaiKhoan");

            var nguoiDung = await _context.NguoiDung
                .FirstOrDefaultAsync(n => n.TenDangNhap == tenDangNhap);

            if (nguoiDung == null)
                return RedirectToAction("DangNhap", "TaiKhoan");

            return View(nguoiDung);
        }
    }
}
