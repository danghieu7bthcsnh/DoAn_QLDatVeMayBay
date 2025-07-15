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
using System.Text.Json;

namespace QLDatVeMayBay.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ThongKeController : Controller
    {
        private readonly QLDatVeMayBayContext _context;

        public ThongKeController(QLDatVeMayBayContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? tuNgay, DateTime? denNgay, string vaiTro, string trangThai, string hangHK)
        {
            tuNgay ??= DateTime.Now.AddMonths(-1);
            denNgay ??= DateTime.Now;

            ViewData["TuNgay"] = tuNgay.Value.ToString("yyyy-MM-dd");
            ViewData["DenNgay"] = denNgay.Value.ToString("yyyy-MM-dd");
            ViewData["VaiTro"] = vaiTro;
            ViewData["TrangThai"] = trangThai;
            ViewData["HangHK"] = hangHK;

            ViewData["VaiTroList"] = new List<SelectListItem>
    {
        new SelectListItem { Text = "Admin", Value = "Admin" },
        new SelectListItem { Text = "Khách hàng", Value = "KhachHang" }
    };

            ViewData["TrangThaiList"] = new List<SelectListItem>
    {
        new SelectListItem { Text = "Hoạt động", Value = "HoatDong" },
        new SelectListItem { Text = "Đã khóa", Value = "DaKhoa" }
    };

            var hangHKList = await _context.MayBay.Select(m => m.TenHangHK).Distinct().ToListAsync();
            ViewData["HangHangKhong"] = hangHKList.Select(h => new SelectListItem { Text = h, Value = h }).ToList();

            var nguoiDungQuery = _context.NguoiDung.Include(n => n.TaiKhoan).AsQueryable();
            if (!string.IsNullOrEmpty(vaiTro))
                nguoiDungQuery = nguoiDungQuery.Where(n => n.TaiKhoan.VaiTro == vaiTro);
            if (!string.IsNullOrEmpty(trangThai))
                nguoiDungQuery = nguoiDungQuery.Where(n => n.TaiKhoan.TrangThaiTK == trangThai);

            var nguoiDungMoi = await nguoiDungQuery.CountAsync();

            var chuyenBayQuery = _context.ChuyenBay.Include(c => c.MayBay).AsQueryable();
            if (!string.IsNullOrEmpty(hangHK))
                chuyenBayQuery = chuyenBayQuery.Where(c => c.MayBay.TenHangHK == hangHK);

            var tongChuyenBay = await chuyenBayQuery.CountAsync();

            var veQuery = _context.VeMayBay
                .Include(v => v.ChuyenBay)
                    .ThenInclude(c => c.MayBay)
                .Include(v => v.ChuyenBay)
                    .ThenInclude(c => c.SanBayDiInfo)
                .Where(v => v.ThoiGianDat >= tuNgay && v.ThoiGianDat <= denNgay)
                .AsQueryable();

            if (!string.IsNullOrEmpty(hangHK))
                veQuery = veQuery.Where(v => v.ChuyenBay.MayBay.TenHangHK == hangHK);

            var tongVe = await veQuery.CountAsync();

            var tongDoanhThu = await _context.ThanhToan
                .Where(t => t.ThoiGianGiaoDich >= tuNgay && t.ThoiGianGiaoDich <= denNgay)
                .SumAsync(t => (decimal?)t.SoTien) ?? 0;

            var doanhThuTheoThang = await _context.ThanhToan
                .Where(t => t.ThoiGianGiaoDich >= tuNgay && t.ThoiGianGiaoDich <= denNgay)
                .GroupBy(t => new { t.ThoiGianGiaoDich.Year, t.ThoiGianGiaoDich.Month })
                .Select(g => new
                {
                    Thang = g.Key.Month + "/" + g.Key.Year,
                    Tong = g.Sum(t => t.SoTien)
                }).ToListAsync();
            ViewData["DataDoanhThuTheoThang"] = JsonSerializer.Serialize(doanhThuTheoThang);

            var datTheoSanBay = await veQuery
                .GroupBy(v => v.ChuyenBay.SanBayDiInfo.TenSanBay)
                .Select(g => new { SanBay = g.Key, SoLuong = g.Count() })
                .ToListAsync();
            ViewData["DataDatTheoSanBay"] = JsonSerializer.Serialize(datTheoSanBay);

            var veHuyTheoThang = await _context.VeMayBay
                .Where(v => v.ThoiGianDat >= tuNgay && v.ThoiGianDat <= denNgay)
                .GroupBy(v => new { v.ThoiGianDat.Year, v.ThoiGianDat.Month })
                .Select(g => new
                {
                    Thang = g.Key.Month + "/" + g.Key.Year,
                    SoVeHuy = g.Count(v => v.TrangThaiVe == "Đã huỷ"),
                    Tong = g.Count()
                }).ToListAsync();

            var tyLeHuy = veHuyTheoThang.Select(x => new
            {
                x.Thang,
                TyLe = x.Tong == 0 ? 0 : Math.Round(x.SoVeHuy * 100.0 / x.Tong, 2)
            }).ToList();
            ViewData["DataVeHuyTheoThang"] = JsonSerializer.Serialize(tyLeHuy);

            var doanhThuTheoPTTT = await _context.ThanhToan
                .Where(t => t.ThoiGianGiaoDich >= tuNgay && t.ThoiGianGiaoDich <= denNgay)
                .GroupBy(t => new { t.ThoiGianGiaoDich.Year, t.ThoiGianGiaoDich.Month })
                .Select(g => new
                {
                    Thang = g.Key.Month + "/" + g.Key.Year,
                    ChiTiet = g.GroupBy(t => t.PhuongThuc)
                        .Select(pt => new
                        {
                            Ten = pt.Key,
                            Tong = pt.Sum(t => t.SoTien)
                        }).ToList()
                }).ToListAsync();
            ViewData["DataDoanhThuTheoPTTT"] = JsonSerializer.Serialize(doanhThuTheoPTTT);

            var bangChiTiet = await _context.ThanhToan
                .Where(t => t.ThoiGianGiaoDich >= tuNgay && t.ThoiGianGiaoDich <= denNgay)
                .GroupBy(t => new { t.ThoiGianGiaoDich.Year, t.ThoiGianGiaoDich.Month })
                .Select(g => new
                {
                    Thang = g.Key.Month + "/" + g.Key.Year,
                    DoanhThu = g.Sum(t => t.SoTien),
                    SoVe = _context.VeMayBay.Count(v => v.ThoiGianDat.Year == g.Key.Year && v.ThoiGianDat.Month == g.Key.Month),
                    NguoiDungMoi = _context.NguoiDung.Count(n => n.TaiKhoan.NgayTao.HasValue && n.TaiKhoan.NgayTao.Value.Year == g.Key.Year && n.TaiKhoan.NgayTao.Value.Month == g.Key.Month)
                }).ToListAsync();

            ViewData["BangChiTiet"] = bangChiTiet;
            ViewData["TongNguoiDung"] = nguoiDungMoi;
            ViewData["TongChuyenBay"] = tongChuyenBay;
            ViewData["TongVe"] = tongVe;
            ViewData["TongDoanhThu"] = tongDoanhThu;

            return View();
        }

        public IActionResult XuatExcel(DateTime? tuNgay, DateTime? denNgay)
        {
            var fromDate = tuNgay ?? new DateTime(DateTime.Today.Year, 1, 1);
            var toDate = denNgay?.AddDays(1).AddTicks(-1) ?? DateTime.Today.AddDays(1).AddTicks(-1);

            var chiTiet = _context.ThanhToan
                .Where(t => t.ThoiGianGiaoDich >= fromDate && t.ThoiGianGiaoDich <= toDate && t.TrangThaiThanhToan == "Đã thanh toán")
                .ToList()
                .GroupBy(t => new { t.ThoiGianGiaoDich.Year, t.ThoiGianGiaoDich.Month })
                .Select(g => new
                {
                    Thang = $"{g.Key.Month:D2}/{g.Key.Year}",
                    DoanhThu = g.Sum(t => t.SoTien),
                    SoVe = _context.VeMayBay.Count(v => v.ThoiGianDat.Year == g.Key.Year && v.ThoiGianDat.Month == g.Key.Month),
                    NguoiDungMoi = _context.TaiKhoan.Count(t => t.NgayTao.HasValue && t.NgayTao.Value.Year == g.Key.Year && t.NgayTao.Value.Month == g.Key.Month && t.TrangThaiTK == "Hoạt động")
                })
                .ToList();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("ThongKe");
            worksheet.Cells.LoadFromCollection(chiTiet, true);

            var bytes = package.GetAsByteArray();
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ThongKe.xlsx");
        }

        public IActionResult XuatPDF(DateTime? tuNgay, DateTime? denNgay)
        {
            var fromDate = tuNgay ?? new DateTime(DateTime.Today.Year, 1, 1);
            var toDate = denNgay?.AddDays(1).AddTicks(-1) ?? DateTime.Today.AddDays(1).AddTicks(-1);

            var chiTiet = _context.ThanhToan
                .Where(t => t.ThoiGianGiaoDich >= fromDate && t.ThoiGianGiaoDich <= toDate && t.TrangThaiThanhToan == "Đã thanh toán")
                .ToList()
                .GroupBy(t => new { t.ThoiGianGiaoDich.Year, t.ThoiGianGiaoDich.Month })
                .Select(g => new
                {
                    Thang = $"{g.Key.Month:D2}/{g.Key.Year}",
                    DoanhThu = g.Sum(t => t.SoTien),
                    SoVe = _context.VeMayBay.Count(v => v.ThoiGianDat.Year == g.Key.Year && v.ThoiGianDat.Month == g.Key.Month),
                    NguoiDungMoi = _context.TaiKhoan.Count(t => t.NgayTao.HasValue && t.NgayTao.Value.Year == g.Key.Year && t.NgayTao.Value.Month == g.Key.Month && t.TrangThaiTK == "Hoạt động")
                })
                .ToList();

            using var ms = new MemoryStream();
            using var writer = new PdfWriter(ms);
            using var pdf = new PdfDocument(writer);
            var doc = new Document(pdf, PageSize.A4);
            var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            doc.SetFont(font);
            doc.Add(new Paragraph("Báo cáo thống kê"));

            var table = new Table(4).UseAllAvailableWidth();
            table.AddHeaderCell("Tháng");
            table.AddHeaderCell("Doanh thu");
            table.AddHeaderCell("Số vé");
            table.AddHeaderCell("Người dùng mới");

            foreach (var item in chiTiet)
            {
                table.AddCell(item.Thang);
                table.AddCell(item.DoanhThu.ToString("N0"));
                table.AddCell(item.SoVe.ToString());
                table.AddCell(item.NguoiDungMoi.ToString());
            }

            doc.Add(table);
            doc.Close();

            return File(ms.ToArray(), "application/pdf", "ThongKe.pdf");
        }

        public async Task<IActionResult> GuiBaoCaoQuaEmail(string email, DateTime? tuNgay, DateTime? denNgay)
        {
            var pdfFile = XuatPDF(tuNgay, denNgay) as FileContentResult;
            if (pdfFile == null)
                return BadRequest("Không thể tạo PDF.");

            using var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("your-email@gmail.com", "your-app-password"),
                EnableSsl = true
            };

            var mail = new MailMessage("your-email@gmail.com", email)
            {
                Subject = "Báo cáo thống kê doanh thu",
                Body = "Vui lòng xem báo cáo thống kê trong file đính kèm."
            };

            mail.Attachments.Add(new Attachment(new MemoryStream(pdfFile.FileContents), "ThongKe.pdf"));
            await smtp.SendMailAsync(mail);

            return Ok("Đã gửi báo cáo qua email.");
        }
    }
}
