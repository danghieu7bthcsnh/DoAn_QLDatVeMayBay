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