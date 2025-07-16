using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLDatVeMayBay.Data;
using QLDatVeMayBay.Models;
using QLDatVeMayBay.Helper;
using QLDatVeMayBay.Services;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace QLDatVeMayBay.Controllers
{
    public class DatVeController : Controller
    {
        private readonly QLDatVeMayBayContext _context;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public DatVeController(QLDatVeMayBayContext context, IConfiguration configuration, EmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<IActionResult> ChonGhe(int idChuyenBay)
        {
            var chuyenBay = await _context.ChuyenBay
                .Include(cb => cb.MayBay).ThenInclude(mb => mb.LoaiMayBay)
                .FirstOrDefaultAsync(cb => cb.IDChuyenBay == idChuyenBay);

            var gheDaDat = await _context.VeMayBay
                .Where(v => v.IDChuyenBay == idChuyenBay)
                .Select(v => v.IDGhe)
                .ToListAsync();

            var model = new DatGhe
            {
                IDChuyenBay = idChuyenBay,
                TongSoGhe = chuyenBay.MayBay.LoaiMayBay.TongSoGhe,
                GheDaDat = gheDaDat
            };

            return View(model);
        }

        public IActionResult XacNhanVe(int idChuyenBay, int idGhe)
        {
            var idNguoiDung = HttpContext.Session.GetInt32("IDNguoiDung");
            if (idNguoiDung == null) return RedirectToAction("DangNhap", "TaiKhoan");

            var nguoiDung = _context.NguoiDung.FirstOrDefault(x => x.IDNguoiDung == idNguoiDung);
            var chuyenBay = _context.ChuyenBay.Include(x => x.MayBay).FirstOrDefault(x => x.IDChuyenBay == idChuyenBay);
            var sanBayDi = _context.SanBay.FirstOrDefault(x => x.IDSanBay == chuyenBay.SanBayDi);
            var sanBayDen = _context.SanBay.FirstOrDefault(x => x.IDSanBay == chuyenBay.SanBayDen);

            var thongTinVe = new ThongTinVe
            {
                IDNguoiDung = nguoiDung.IDNguoiDung,
                HoTen = nguoiDung.HoTen,
                GioiTinh = nguoiDung.GioiTinh,
                IDChuyenBay = chuyenBay.IDChuyenBay,
                TenHangHK = chuyenBay.MayBay.TenHangHK,
                GioCatCanh = chuyenBay.GioCatCanh,
                GioHaCanh = chuyenBay.GioHaCanh,
                SanBayDi = sanBayDi.IDSanBay,
                SanBayDen = sanBayDen.IDSanBay,
                TenSanBayDi = sanBayDi.TenSanBay,
                TenSanBayDen = sanBayDen.TenSanBay,
                IDGhe = idGhe.ToString(),
                GiaVe = chuyenBay.GiaVe
            };

            return View("XacNhanVe", thongTinVe);
        }

        [HttpGet]
        public IActionResult ThanhToan(int idChuyenBay, int idGhe)
        {
            var idNguoiDung = HttpContext.Session.GetInt32("IDNguoiDung");
            if (idNguoiDung == null) return RedirectToAction("DangNhap", "TaiKhoan");

            var chuyenBay = _context.ChuyenBay.Find(idChuyenBay);
            var giaVe = chuyenBay?.GiaVe ?? 0;

            var model = new ThongTinThanhToan
            {
                Ve = new VeMayBay
                {
                    IDNguoiDung = idNguoiDung.Value,
                    IDChuyenBay = idChuyenBay,
                    IDGhe = idGhe,
                    ThoiGianDat = DateTime.Now,
                    TrangThaiVe = "Chưa thanh toán"
                },
                SoTien = giaVe
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ThanhToan(ThongTinThanhToan model)
        {
            var nguoiDung = await _context.NguoiDung.FindAsync(model.Ve.IDNguoiDung);
            if (nguoiDung == null) return NotFound();

            var otp = new Random().Next(100000, 999999).ToString();
            HttpContext.Session.SetString("OTP", otp);
            HttpContext.Session.SetString("OTP_Expires", DateTime.Now.AddMinutes(2).ToString());
            HttpContext.Session.Set("VeTemp", JsonSerializer.SerializeToUtf8Bytes(model));

            await _emailService.SendEmailAsync(nguoiDung.Email, "Xác nhận OTP", $"Mã OTP: {otp}");

            return View("NhapOTP", model);
        }
        [HttpPost]
        public async Task<IActionResult> KiemTraOTP(ThongTinThanhToan model)
        {
            // Lấy mã OTP và thời gian hết hạn từ session
            var otp = HttpContext.Session.GetString("OTP");
            var otpExpStr = HttpContext.Session.GetString("OTP_Expires");

            if (string.IsNullOrEmpty(otp) || string.IsNullOrEmpty(otpExpStr))
            {
                ModelState.AddModelError("", "Phiên OTP không hợp lệ. Vui lòng thử lại.");
                return View("NhapOTP", model);
            }

            var otpExp = DateTime.Parse(otpExpStr);

            if (model.MaOTP != otp || DateTime.Now > otpExp)
            {
                ModelState.AddModelError("MaOTP", "OTP không hợp lệ hoặc đã hết hạn.");
                return View("NhapOTP", model);
            }

            // Deserialize dữ liệu ThongTinThanhToan từ session
            var veData = HttpContext.Session.Get("VeTemp");
            var fullModel = JsonSerializer.Deserialize<ThongTinThanhToan>(veData);
            var ve = fullModel.Ve;
            ve.TrangThaiVe = "Đã đặt";

            // Lưu vé vào DB
            _context.VeMayBay.Add(ve);
            await _context.SaveChangesAsync();

            // Lưu thông tin thanh toán
            var thanhToan = new ThanhToan
            {
                IDVe = ve.IDVe,
                SoTien = fullModel.SoTien,
                PhuongThuc = fullModel.PhuongThuc,
                ThoiGianGiaoDich = DateTime.Now,
                TrangThaiThanhToan = "Thành công"
            };
            _context.ThanhToan.Add(thanhToan);
            await _context.SaveChangesAsync();

            // Lấy thông tin chuyến bay
            var chuyenBay = await _context.ChuyenBay
                .Include(cb => cb.MayBay)
                .Include(cb => cb.SanBayDiInfo)
                .Include(cb => cb.SanBayDenInfo)
                .FirstOrDefaultAsync(cb => cb.IDChuyenBay == ve.IDChuyenBay);

            // Lấy thông tin người dùng
            var nguoiDung = await _context.NguoiDung.FindAsync(ve.IDNguoiDung);
            if (nguoiDung == null)
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }

            // Tạo mã QR
            var qrText = $"""
👤 Mã KH: {ve.IDNguoiDung}
✈ Chuyến bay: {ve.IDChuyenBay}
📍 {chuyenBay?.SanBayDiInfo?.TenSanBay} → {chuyenBay?.SanBayDenInfo?.TenSanBay}
🛫 Cất cánh: {chuyenBay?.GioCatCanh:dd/MM/yyyy HH:mm}
🛬 Hạ cánh: {chuyenBay?.GioHaCanh:dd/MM/yyyy HH:mm}
🛩 Loại máy bay: {chuyenBay?.MayBay?.TenHangHK}
💺 Ghế: G{ve.IDGhe} | Hạng: {ve.HangGhe ?? "Không rõ"}
🎫 Loại vé: {ve.LoaiVe ?? "Thường"}
🆔 Mã vé: {ve.IDVe}
""";

            var qrBase64 = QRCodeHelper.GenerateQRCodeBase64(qrText);

            // Gửi email xác nhận
            string emailHtml = $"""
    <h2>✅ Đặt vé thành công!</h2>
    <p>Chào <strong>{nguoiDung.HoTen}</strong>,</p>
    <p>Bạn đã đặt vé thành công. Dưới đây là thông tin vé:</p>
    <ul>
        <li><strong>Chuyến bay:</strong> {ve.IDChuyenBay} - {chuyenBay?.MayBay?.TenHangHK}</li>
        <li><strong>Ghế:</strong> G{ve.IDGhe} | Hạng: {ve.HangGhe ?? "Không rõ"}</li>
        <li><strong>Loại vé:</strong> {ve.LoaiVe ?? "Thường"}</li>
        <li><strong>Điểm đi:</strong> {chuyenBay?.SanBayDiInfo?.TenSanBay}</li>
        <li><strong>Điểm đến:</strong> {chuyenBay?.SanBayDenInfo?.TenSanBay}</li>
        <li><strong>Cất cánh:</strong> {chuyenBay?.GioCatCanh:dd/MM/yyyy HH:mm}</li>
        <li><strong>Hạ cánh:</strong> {chuyenBay?.GioHaCanh:dd/MM/yyyy HH:mm}</li>
    </ul>
    <p><strong>Mã QR của bạn:</strong></p>
    <img src='data:image/png;base64,{qrBase64}' width='220' />
    """;

            await _emailService.SendEmailAsync(
        nguoiDung.Email,
        "✅ Xác nhận đặt vé thành công",
        emailHtml
    );

            // Gán dữ liệu cho ViewBag
            ViewBag.QRBase64 = qrBase64;
            ViewBag.Ve = ve;
            ViewBag.ThanhToan = thanhToan;
            ViewBag.ChuyenBay = chuyenBay;
            ViewBag.NguoiDung = nguoiDung;

            return View("ThanhToanThanhCong", fullModel);
        }


    }
}

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using QLDatVeMayBay.Data;
//using QLDatVeMayBay.Models;
//using QLDatVeMayBay.Helper;
//using QRCoder;
//using System.Text.Json;
//using QLDatVeMayBay.Services;
//using MailKit.Security;
//using MimeKit;
//using QLDatVeMayBay.ViewModels;

//namespace QLDatVeMayBay.Controllers
//{
//    public class DatVeController : Controller
//    {
//        private readonly QLDatVeMayBayContext _context;
//        private readonly IConfiguration _configuration;
//        private readonly EmailService _emailService;

//        public DatVeController(QLDatVeMayBayContext context, IConfiguration configuration, EmailService emailService)
//        {
//            _context = context;
//            _configuration = configuration;
//            _emailService = emailService;
//        }

//        public async Task<IActionResult> ChonGhe(int idChuyenBay)
//        {
//            var chuyenBay = await _context.ChuyenBay
//                .Include(cb => cb.MayBay)
//                    .ThenInclude(mb => mb.LoaiMayBay)
//                .FirstOrDefaultAsync(cb => cb.IDChuyenBay == idChuyenBay);

//            if (chuyenBay == null || chuyenBay.MayBay?.LoaiMayBay == null)
//                return NotFound("Không tìm thấy chuyến bay hoặc loại máy bay.");

//            var tongSoGhe = chuyenBay.MayBay.LoaiMayBay.TongSoGhe;
//            var gheDaDat = await _context.VeMayBay
//                .Where(v => v.IDChuyenBay == idChuyenBay)
//                .Select(v => v.IDGhe)
//                .ToListAsync();

//            var model = new DatGhe
//            {
//                IDChuyenBay = idChuyenBay,
//                TongSoGhe = tongSoGhe,
//                GheDaDat = gheDaDat
//            };

//            return View("ChonGhe", model);
//        }

//        public IActionResult XacNhanVe(int idChuyenBay, int idGhe)
//        {
//            var idNguoiDung = HttpContext.Session.GetInt32("IDNguoiDung");
//            if (idNguoiDung == null) return RedirectToAction("DangNhap", "TaiKhoan");

//            var nguoiDung = _context.NguoiDung.FirstOrDefault(x => x.IDNguoiDung == idNguoiDung);
//            var chuyenBay = _context.ChuyenBay.Include(x => x.MayBay).FirstOrDefault(x => x.IDChuyenBay == idChuyenBay);
//            var sanBayDi = _context.SanBay.FirstOrDefault(x => x.IDSanBay == chuyenBay.SanBayDi);
//            var sanBayDen = _context.SanBay.FirstOrDefault(x => x.IDSanBay == chuyenBay.SanBayDen);

//            var thongTinVe = new ThongTinVe
//            {
//                IDNguoiDung = nguoiDung.IDNguoiDung,
//                HoTen = nguoiDung.HoTen,
//                GioiTinh = nguoiDung.GioiTinh,
//                IDChuyenBay = chuyenBay.IDChuyenBay,
//                TenHangHK = chuyenBay.MayBay.TenHangHK,
//                GioCatCanh = chuyenBay.GioCatCanh,
//                GioHaCanh = chuyenBay.GioHaCanh,
//                SanBayDi = sanBayDi.IDSanBay,
//                SanBayDen = sanBayDen.IDSanBay,
//                TenSanBayDi = sanBayDi.TenSanBay,
//                TenSanBayDen = sanBayDen.TenSanBay,
//                IDGhe = idGhe.ToString(),
//                GiaVe = chuyenBay.GiaVe
//            };

//            return View("XacNhanVe", thongTinVe);
//        }

//        [HttpGet]
//        public IActionResult ThanhToan(int idChuyenBay, int idGhe)
//        {
//            var idNguoiDung = HttpContext.Session.GetInt32("IDNguoiDung");
//            if (idNguoiDung == null) return RedirectToAction("DangNhap", "TaiKhoan");

//            var chuyenBay = _context.ChuyenBay.Find(idChuyenBay);
//            var giaVe = chuyenBay?.GiaVe ?? 0;

//            var model = new ThongTinThanhToan
//            {
//                Ve = new VeMayBay
//                {
//                    IDNguoiDung = idNguoiDung.Value,
//                    IDChuyenBay = idChuyenBay,
//                    IDGhe = idGhe,
//                    ThoiGianDat = DateTime.Now,
//                    TrangThaiVe = "Chưa thanh toán"
//                },
//                SoTien = giaVe
//            };

//            return View("ThanhToan", model);
//        }

//        [HttpPost]
//        public async Task<IActionResult> ThanhToan(ThongTinThanhToan model)
//        {
//            if (!ModelState.IsValid)
//            {
//                ViewBag.Error = "Dữ liệu không hợp lệ.";
//                return View("ThanhToan", model);
//            }

//            var nguoiDung = await _context.NguoiDung.FindAsync(model.Ve.IDNguoiDung);
//            if (nguoiDung == null)
//                return NotFound("Không tìm thấy người dùng.");

//            var otp = new Random().Next(100000, 999999).ToString();
//            HttpContext.Session.SetString("OTP", otp);
//            HttpContext.Session.SetString("OTP_Expires", DateTime.Now.AddMinutes(2).ToString());
//            HttpContext.Session.Set("VeTemp", JsonSerializer.SerializeToUtf8Bytes(model));

//            await _context.SaveChangesAsync();

//            try
//            {
//                await _emailService.SendEmailAsync(
//                    nguoiDung.Email,
//                    "Xác nhận OTP thanh toán vé máy bay",
//                    $"Mã OTP của bạn là: {otp}\nVui lòng nhập mã trong vòng 2 phút để hoàn tất thanh toán."
//                );
//            }
//            catch (Exception ex)
//            {
//                ViewBag.Error = "Lỗi gửi email: " + ex.Message;
//                return View(model);
//            }

//            return View("NhapOTP");
//        }

//        // 6. Xác nhận OTP và thanh toán thành công
//        [HttpPost]
//        public async Task<IActionResult> XacNhanOTPThanhToan(string otp)
//        {
//            var otpSaved = HttpContext.Session.GetString("OTP");
//            var expiredStr = HttpContext.Session.GetString("OTP_Expires");

//            if (otpSaved == null || expiredStr == null || otp != otpSaved)
//                return View("XacNhanOTPThanhCong", "Mã OTP không hợp lệ hoặc đã hết hạn.");

//            if (DateTime.TryParse(expiredStr, out var expiredTime) && DateTime.Now > expiredTime)
//                return View("XacNhanOTPThanhCong", "Mã OTP đã hết hạn.");

//            var modelBytes = HttpContext.Session.Get("VeTemp");
//            var thongTin = JsonSerializer.Deserialize<ThongTinThanhToan>(modelBytes);
//            thongTin.Ve.TrangThaiVe = "Đã thanh toán";

//            _context.VeMayBay.Add(thongTin.Ve);
//            await _context.SaveChangesAsync();

//            return RedirectToAction("ThanhToanThanhCong");
//        }

//        public IActionResult ThanhToanThanhCong()
//        {
//            return View("ThanhToanThanhCong");
//        }
//    }
//}
////using Microsoft.AspNetCore.Mvc;
////using Microsoft.EntityFrameworkCore;
////using QLDatVeMayBay.Data;
////using QLDatVeMayBay.Models;
////using QLDatVeMayBay.Helper;
////using QRCoder;
////using System.Text.Json;



////using QLDatVeMayBay.Services;
////using MailKit.Security;
////using MimeKit;



////namespace QLDatVeMayBay.Controllers
////{
////    public class DatVeController : Controller
////    {
////        private readonly QLDatVeMayBayContext _context;
////        private readonly IConfiguration _configuration;
////        private readonly EmailService _emailService;

////        public DatVeController(QLDatVeMayBayContext context, IConfiguration configuration, EmailService emailService)
////        {
////            _context = context;
////            _configuration = configuration;
////            _emailService = emailService;
////        }



////        public async Task<IActionResult> ChonGhe(int idChuyenBay)
////        {
////            var chuyenBay = await _context.ChuyenBay
////                .Include(cb => cb.MayBay)
////                    .ThenInclude(mb => mb.LoaiMayBay)
////                .FirstOrDefaultAsync(cb => cb.IDChuyenBay == idChuyenBay);

////            if (chuyenBay == null || chuyenBay.MayBay?.LoaiMayBay == null)
////                return NotFound("Không tìm thấy chuyến bay hoặc loại máy bay.");

////            var tongSoGhe = chuyenBay.MayBay.LoaiMayBay.TongSoGhe;
////            var gheDaDat = await _context.VeMayBay
////                .Where(v => v.IDChuyenBay == idChuyenBay)
////                .Select(v => v.IDGhe)
////                .ToListAsync();

////            var model = new DatGhe
////            {
////                IDChuyenBay = idChuyenBay,
////                TongSoGhe = tongSoGhe,
////                GheDaDat = gheDaDat
////            };

////            return View(model); // ✅ Gửi model về View
////        }




////        public IActionResult XacNhanVe(int idChuyenBay, int idGhe)
////        {
////            var idNguoiDung = HttpContext.Session.GetInt32("IDNguoiDung");
////            if (idNguoiDung == null) return RedirectToAction("DangNhap", "TaiKhoan");

////            var nguoiDung = _context.NguoiDung.FirstOrDefault(x => x.IDNguoiDung == idNguoiDung);
////            var chuyenBay = _context.ChuyenBay.Include(x => x.MayBay).FirstOrDefault(x => x.IDChuyenBay == idChuyenBay);
////            // Lấy sân bay đi và đến
////            var sanBayDi = _context.SanBay.FirstOrDefault(x => x.IDSanBay == chuyenBay.SanBayDi);
////            var sanBayDen = _context.SanBay.FirstOrDefault(x => x.IDSanBay == chuyenBay.SanBayDen);
////            // Tạo mã QR (ghi thông tin cơ bản)
////            var qrData = $"{nguoiDung.HoTen} - Chuyến bay {chuyenBay.IDChuyenBay} - Ghế G{idGhe}";
////            // var qrBase64 = GenerateQRCode(qrData);
////            var thongTinVe = new ThongTinVe
////            {
////                IDNguoiDung = nguoiDung.IDNguoiDung,
////                HoTen = nguoiDung.HoTen,
////                GioiTinh = nguoiDung.GioiTinh,

////                IDChuyenBay = chuyenBay.IDChuyenBay,
////                TenHangHK = chuyenBay.MayBay.TenHangHK,
////                GioCatCanh = chuyenBay.GioCatCanh,
////                GioHaCanh = chuyenBay.GioHaCanh,
////                SanBayDi = sanBayDi.IDSanBay, // giữ ID dạng string nếu cần
////                SanBayDen = sanBayDen.IDSanBay,
////                TenSanBayDi = sanBayDi.TenSanBay,            // tên thật để hiển thị
////                TenSanBayDen = sanBayDen.TenSanBay,
////                IDGhe = idGhe.ToString(),
////                GiaVe = chuyenBay.GiaVe,
////                //QRBase64 = qrBase64
////            };

////            // Sinh QR chứa thông tin vé
////            var qrContent = $"Tên: {thongTinVe.HoTen} | Chuyến: {thongTinVe.IDChuyenBay} | Ghế: G{idGhe} | Từ: {thongTinVe.SanBayDi} → {thongTinVe.SanBayDen} | {thongTinVe.GioCatCanh:dd/MM/yyyy HH:mm}";
////            //ViewBag.QRCodeBase64 = QRCodeHelper.GenerateQRCodeBase64(qrContent);

////            return View("XacNhanVe", thongTinVe);
////        }
////        [HttpGet]
////        public IActionResult ThanhToan(int idChuyenBay, int idGhe)
////        {
////            var idNguoiDung = HttpContext.Session.GetInt32("IDNguoiDung");
////            if (idNguoiDung == null) return RedirectToAction("DangNhap", "TaiKhoan");

////            var chuyenBay = _context.ChuyenBay.Find(idChuyenBay);
////            var giaVe = chuyenBay?.GiaVe ?? 0;

////            var model = new ThongTinThanhToan
////            {
////                Ve = new VeMayBay
////                {
////                    IDNguoiDung = idNguoiDung.Value,
////                    IDChuyenBay = idChuyenBay,
////                    IDGhe = idGhe,
////                    ThoiGianDat = DateTime.Now,
////                    TrangThaiVe = "Chưa thanh toán"
////                },
////                SoTien = giaVe
////            };

////            return View(model); // View nhập ngân hàng
////        }
////        [HttpPost]
////        public async Task<IActionResult> ThanhToan(ThongTinThanhToan model)
////        {
////            if (!ModelState.IsValid)
////            {
////                ViewBag.Error = "Dữ liệu không hợp lệ.";
////                return View(model);
////            }

////            var nguoiDung = await _context.NguoiDung.FindAsync(model.Ve.IDNguoiDung);
////            if (nguoiDung == null)
////                return NotFound("Không tìm thấy người dùng.");

////            var otp = new Random().Next(100000, 999999).ToString();
////            HttpContext.Session.SetString("OTP", otp);
////            HttpContext.Session.SetString("OTP_Expires", DateTime.Now.AddMinutes(2).ToString());
////            HttpContext.Session.Set("VeTemp", JsonSerializer.SerializeToUtf8Bytes(model));

////            try
////            {
////                await _emailService.SendEmailAsync(
////                    nguoiDung.Email,
////                    "Xác nhận OTP thanh toán vé máy bay",
////                    $"Mã OTP của bạn là: {otp}\nVui lòng nhập mã trong vòng 2 phút để hoàn tất thanh toán."
////                );
////            }
////            catch (Exception ex)
////            {
////                ViewBag.Error = "Lỗi gửi email: " + ex.Message;
////                // Ghi log nếu cần
////                ModelState.AddModelError("", "Không thể gửi email. Vui lòng kiểm tra lại cấu hình Email.");
////                ViewBag.LoiEmail = ex.Message; // gửi lỗi ra view nếu cần hiển thị chi tiết
////                return View(model);
////            }

////            return View("NhapOTP", model);
////        }
////        [HttpPost]

////        [HttpPost]
////        public IActionResult XacNhanOTP(string otp)
////        {
////            var otpSaved = HttpContext.Session.GetString("OTP");
////            var expiresStr = HttpContext.Session.GetString("OTP_Expires");

////            if (otpSaved == null || expiresStr == null)
////                return Content("Phiên xác thực OTP đã hết hạn. Vui lòng thử lại.");

////            if (DateTime.TryParse(expiresStr, out var expires))
////            {
////                if (DateTime.Now > expires)
////                    return Content("Mã OTP đã hết hạn.");
////            }

////            if (otp != otpSaved)
////                return Content("Mã OTP không đúng.");

////            // Lấy lại thông tin vé
////            var veData = HttpContext.Session.Get("VeTemp");
////            if (veData == null) return Content("Dữ liệu không hợp lệ.");

////            var thongTinVe = JsonSerializer.Deserialize<ThongTinThanhToan>(veData);
////            thongTinVe.Ve.TrangThaiVe = "Đã thanh toán";

////            _context.VeMayBay.Add(thongTinVe.Ve);
////            _context.SaveChanges();

////            return RedirectToAction("ThanhToanThanhCong");
////        }

////        [HttpPost]
////        public async Task<IActionResult> KiemTraOTP(ThongTinThanhToan model)
////        {
////            var otp = HttpContext.Session.GetString("OTP");
////            var otpExp = DateTime.Parse(HttpContext.Session.GetString("OTP_Expires"));
////            if (model.MaOTP != otp || DateTime.Now > otpExp)
////            {
////                ModelState.AddModelError("MaOTP", "Mã OTP sai hoặc đã hết hạn.");
////                return View("NhapOTP", model);
////            }

////            // Lấy lại dữ liệu tạm từ session
////            var modelBytes = HttpContext.Session.Get("VeTemp");
////            var fullModel = JsonSerializer.Deserialize<ThongTinThanhToan>(modelBytes);
////            var ve = fullModel.Ve;

////            // Cập nhật trạng thái
////            ve.TrangThaiVe = "Đã đặt";
////            _context.VeMayBay.Add(ve);
////            await _context.SaveChangesAsync();

////            // Lưu thông tin thanh toán
////            var thanhToan = new ThanhToan
////            {
////                IDVe = ve.IDVe,
////                SoTien = fullModel.SoTien,
////                PhuongThuc = fullModel.PhuongThuc,
////                ThoiGianGiaoDich = DateTime.Now,
////                TrangThaiThanhToan = "Thành công"
////            };
////            _context.ThanhToan.Add(thanhToan);
////            await _context.SaveChangesAsync();

////            // ✅ Lấy thêm thông tin liên quan vé
////            var nguoiDung = await _context.NguoiDung.FirstOrDefaultAsync(x => x.IDNguoiDung == ve.IDNguoiDung);
////            var chuyenBay = await _context.ChuyenBay
////                .Include(cb => cb.MayBay)
////                .Include(cb => cb.SanBayDiInfo)
////                .Include(cb => cb.SanBayDenInfo)
////                .FirstOrDefaultAsync(cb => cb.IDChuyenBay == ve.IDChuyenBay);

////            var tenSanBayDi = chuyenBay?.SanBayDiInfo?.TenSanBay ?? "Không rõ";
////            var tenSanBayDen = chuyenBay?.SanBayDenInfo?.TenSanBay ?? "Không rõ";
////            var hangHK = chuyenBay?.MayBay?.TenHangHK ?? "Không rõ";
////            var gioCatCanh = chuyenBay?.GioCatCanh ?? DateTime.Now;
////            var gioHaCanh = chuyenBay?.GioHaCanh ?? DateTime.Now;

////            // ✅ Nội dung QR đầy đủ
////            var noiDungQR = $@"
////🛫 Họ tên: {nguoiDung?.HoTen}
////✈ Chuyến bay: {ve.IDChuyenBay}
////🏢 Hãng: {hangHK}
////📍 Từ: {tenSanBayDi}
////📍 Đến: {tenSanBayDen}
////🕒 Cất cánh: {gioCatCanh:dd/MM/yyyy HH:mm}
////🕓 Hạ cánh: {gioHaCanh:dd/MM/yyyy HH:mm}
////💺 Ghế: G{ve.IDGhe}
////🎟 Hạng: Phổ thông
////🎫 Mã vé: {ve.IDVe}
////";

////            string qrBase64 = QRCodeHelper.GenerateQRCodeBase64(noiDungQR);
////            ViewBag.QRBase64 = qrBase64;

////            ViewBag.Ve = ve;
////            ViewBag.ThanhToan = thanhToan;
////            ViewBag.NguoiDung = nguoiDung;
////            ViewBag.ChuyenBay = chuyenBay;

////            return View("ThanhToanThanhCong");
////        }




////    }
////}