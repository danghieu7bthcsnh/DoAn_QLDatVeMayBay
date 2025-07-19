using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLDatVeMayBay.Data;
using QLDatVeMayBay.Models;
using OfficeOpenXml;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Net;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Geom;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
namespace QLDatVeMayBay.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VeMayBayController : Controller
    {
        private readonly QLDatVeMayBayContext _context;
        private const int PageSize = 10;

        public VeMayBayController(QLDatVeMayBayContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string tuKhoa, string trangThai, int? idChuyenBay, string hangGhe, DateTime? ngayDat, int page = 1)
        {
            var query = _context.VeMayBay
                .Include(v => v.NguoiDung)
                .Include(v => v.ChuyenBay)
                    .ThenInclude(cb => cb.MayBay)
                .Include(v => v.Ghe)
                .AsQueryable();

            // 🔍 Lọc theo từ khoá (Họ tên, Email, ID chuyến bay)
            if (!string.IsNullOrEmpty(tuKhoa))
            {
                tuKhoa = tuKhoa.ToLower();
                query = query.Where(v =>
                    (v.NguoiDung.HoTen != null && v.NguoiDung.HoTen.ToLower().Contains(tuKhoa)) ||
                    (v.NguoiDung.Email != null && v.NguoiDung.Email.ToLower().Contains(tuKhoa)) ||
                    v.ChuyenBay.IDChuyenBay.ToString().Contains(tuKhoa)
                );
            }

            // 🧾 Lọc theo trạng thái vé
            if (!string.IsNullOrEmpty(trangThai))
            {
                query = query.Where(v => v.TrangThaiVe == trangThai);
            }

            // ✈️ Lọc theo chuyến bay
            if (idChuyenBay.HasValue)
            {
                query = query.Where(v => v.IDChuyenBay == idChuyenBay.Value);
            }

            // 💺 Lọc theo hạng ghế
            if (!string.IsNullOrEmpty(hangGhe))
            {
                query = query.Where(v => v.Ghe.HangGhe == hangGhe);
            }

            // 📅 Lọc theo ngày đặt
            if (ngayDat.HasValue)
            {
                DateTime dateOnly = ngayDat.Value.Date;
                query = query.Where(v => v.ThoiGianDat.Date == dateOnly);
            }

            // 📄 Tổng số bản ghi
            int totalItems = await query.CountAsync();
            int PageSize = 10;

            var danhSachVe = await query
                .OrderByDescending(v => v.ThoiGianDat)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            // ViewBag dùng cho bộ lọc & phân trang
            ViewBag.TuKhoa = tuKhoa;
            ViewBag.TrangThai = trangThai;
            ViewBag.IDChuyenBay = idChuyenBay;
            ViewBag.HangGhe = hangGhe;
            ViewBag.NgayDat = ngayDat?.ToString("yyyy-MM-dd");
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / PageSize);

            // Trạng thái vé
            ViewBag.TrangThaiList = new List<SelectListItem>
    {
        new SelectListItem { Text = "✅ Đã thanh toán", Value = "Đã thanh toán" },
        new SelectListItem { Text = "⌛ Chưa thanh toán", Value = "Chưa thanh toán" },
        new SelectListItem { Text = "❌ Đã huỷ", Value = "Đã huỷ" }
    };

            // Danh sách chuyến bay
            ViewBag.ChuyenBayList = new SelectList(
                await _context.ChuyenBay
                    .Include(cb => cb.MayBay)
                    .Select(cb => new
                    {
                        cb.IDChuyenBay,
                        Ten = cb.IDChuyenBay + " - " + cb.MayBay.TenHangHK
                    }).ToListAsync(),
                "IDChuyenBay",
                "Ten"
            );

            // Hạng ghế
            ViewBag.HangGheList = (await _context.GheNgoi
                .Select(g => g.HangGhe)
                .Distinct()
                .ToListAsync())
                .Select(h => new SelectListItem { Value = h, Text = h })
                .ToList();

            return View(danhSachVe);
        }


        public async Task<IActionResult> ChiTiet(int id)
        {
            var ve = await _context.VeMayBay
       .Include(v => v.NguoiDung)
       .Include(v => v.ChuyenBay)
           .ThenInclude(cb => cb.MayBay)
       .Include(v => v.ChuyenBay)
           .ThenInclude(cb => cb.SanBayDiInfo)   // Thêm dòng này
       .Include(v => v.ChuyenBay)
           .ThenInclude(cb => cb.SanBayDenInfo)  // Thêm dòng này
       .Include(v => v.Ghe)
       .FirstOrDefaultAsync(v => v.IDVe == id);

            if (ve == null) return NotFound();
            return View(ve);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var ve = await _context.VeMayBay
                .Include(v => v.NguoiDung)
                .Include(v => v.ChuyenBay).ThenInclude(cb => cb.MayBay)
                .Include(v => v.Ghe)
                .FirstOrDefaultAsync(v => v.IDVe == id);

            if (ve == null) return NotFound();

            // Gửi danh sách trạng thái
            ViewBag.TrangThaiList = GetTrangThaiList(ve.TrangThaiVe);

            return View(ve);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string trangThaiVe)
        {
            var ve = await _context.VeMayBay
                .Include(v => v.NguoiDung)
                .Include(v => v.ChuyenBay).ThenInclude(cb => cb.MayBay)
                .Include(v => v.Ghe)
                .FirstOrDefaultAsync(v => v.IDVe == id);

            if (ve == null) return NotFound();

            var validStatuses = new List<string> { "Đã thanh toán", "Chưa thanh toán", "Đã huỷ" };

            if (string.IsNullOrEmpty(trangThaiVe))
            {
                ModelState.AddModelError("trangThaiVe", "Vui lòng chọn trạng thái.");
            }
            else if (!validStatuses.Contains(trangThaiVe))
            {
                ModelState.AddModelError("trangThaiVe", "Trạng thái không hợp lệ.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.TrangThaiList = GetTrangThaiList(trangThaiVe);
                return View(ve);
            }

            ve.TrangThaiVe = trangThaiVe;
            await _context.SaveChangesAsync();

            TempData["Message"] = "✅ Cập nhật trạng thái vé thành công.";
            return RedirectToAction("Index");
        }

        // ✅ Phương thức tái sử dụng danh sách trạng thái
        private List<SelectListItem> GetTrangThaiList(string? selectedValue = null)
        {
            var list = new List<SelectListItem>
    {
        new SelectListItem { Text = "✅ Đã thanh toán", Value = "Đã thanh toán" },
        new SelectListItem { Text = "⌛ Chưa thanh toán", Value = "Chưa thanh toán" },
        new SelectListItem { Text = "❌ Đã huỷ", Value = "Đã huỷ" }
    };

            foreach (var item in list)
            {
                if (item.Value == selectedValue)
                    item.Selected = true;
            }

            return list;
        }


        [HttpPost]
        public async Task<IActionResult> GuiEmail(int id)
        {
            var ve = await _context.VeMayBay
                .Include(v => v.NguoiDung)
                .Include(v => v.ChuyenBay).ThenInclude(cb => cb.MayBay)
                .Include(v => v.Ghe)
                .FirstOrDefaultAsync(v => v.IDVe == id);

            if (ve == null) return NotFound();

            var message = new MailMessage("your_email@example.com", ve.NguoiDung.Email)
            {
                Subject = "Thông tin vé máy bay của bạn",
                Body = $"Chào {ve.NguoiDung.HoTen},\n\nĐây là thông tin vé của bạn: Mã vé: {ve.IDVe}, Chuyến bay: {ve.IDChuyenBay}, Ghế: {ve.Ghe.HangGhe}, Trạng thái: {ve.TrangThaiVe}, Thời gian đặt: {ve.ThoiGianDat}",
            };

            using var smtp = new SmtpClient("smtp.yourserver.com")
            {
                Credentials = new NetworkCredential("your_email@example.com", "your_password"),
                EnableSsl = true
            };
            await smtp.SendMailAsync(message);

            TempData["Message"] = "Đã gửi email vé thành công.";
            return RedirectToAction("ChiTiet", new { id });
        }

        public async Task<IActionResult> XuatPDF(int id)
        {
            var ve = await _context.VeMayBay
                .Include(v => v.NguoiDung)
                .Include(v => v.ChuyenBay).ThenInclude(cb => cb.MayBay)
                .Include(v => v.Ghe)
                .FirstOrDefaultAsync(v => v.IDVe == id);

            if (ve == null) return NotFound();

            using var stream = new MemoryStream();
            using var writer = new PdfWriter(stream);
            using var pdf = new PdfDocument(writer);
            var a5Size = new PageSize(420, 595); // Kích thước A5 đơn vị point (1pt = 1/72 inch)
            var doc = new Document(pdf, a5Size);
            var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            doc.Add(new Paragraph("VÉ MÁY BAY").SetFontSize(20).SetFont(boldFont).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            doc.Add(new Paragraph($"Mã vé: {ve.IDVe}"));
            doc.Add(new Paragraph($"Họ tên: {ve.NguoiDung?.HoTen}"));
            doc.Add(new Paragraph($"Chuyến bay: {ve.ChuyenBay?.IDChuyenBay}"));
            doc.Add(new Paragraph($"Hãng: {ve.ChuyenBay?.MayBay?.TenHangHK}"));
            doc.Add(new Paragraph($"Ghế: {ve.Ghe?.HangGhe}"));
            doc.Add(new Paragraph($"Trạng thái: {ve.TrangThaiVe}"));
            doc.Add(new Paragraph($"Thời gian đặt: {ve.ThoiGianDat:dd/MM/yyyy HH:mm}"));

            doc.Close();
            stream.Position = 0;

            return File(stream, "application/pdf", $"Ve_{ve.IDVe}.pdf");
        }

        [HttpGet]
        public async Task<IActionResult> XacNhanXoa(int id)
        {
            var ve = await _context.VeMayBay.FindAsync(id);
            if (ve == null) return NotFound();

            _context.VeMayBay.Remove(ve);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Xoá vé thành công.";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> XuatExcel(string tuKhoa, string trangThai, int? idChuyenBay, string hangGhe, DateTime? ngayDat)
        {
            var query = _context.VeMayBay
                .Include(v => v.NguoiDung)
                .Include(v => v.ChuyenBay).ThenInclude(cb => cb.MayBay)
                .Include(v => v.Ghe)
                .AsQueryable();

            var list = await query.ToListAsync();

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("VeMayBay");
            ws.Cells[1, 1].Value = "ID";
            ws.Cells[1, 2].Value = "Người đặt";
            ws.Cells[1, 3].Value = "Chuyến bay";
            ws.Cells[1, 4].Value = "Hạng ghế";
            ws.Cells[1, 5].Value = "Thời gian đặt";
            ws.Cells[1, 6].Value = "Trạng thái";

            for (int i = 0; i < list.Count; i++)
            {
                var v = list[i];
                ws.Cells[i + 2, 1].Value = v.IDVe;
                ws.Cells[i + 2, 2].Value = v.NguoiDung?.HoTen;
                ws.Cells[i + 2, 3].Value = v.ChuyenBay?.IDChuyenBay;
                ws.Cells[i + 2, 4].Value = v.Ghe?.HangGhe;
                ws.Cells[i + 2, 5].Value = v.ThoiGianDat.ToString("dd/MM/yyyy HH:mm");
                ws.Cells[i + 2, 6].Value = v.TrangThaiVe;
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "VeMayBay.xlsx");
        }
    }
}
