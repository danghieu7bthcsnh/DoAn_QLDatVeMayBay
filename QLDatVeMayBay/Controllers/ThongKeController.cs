using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using QLDatVeMayBay.Data;
using QLDatVeMayBay.Models;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;

<<<<<<< Updated upstream
=======


>>>>>>> Stashed changes
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

        public async Task<IActionResult> Index(DateTime? tuNgay, DateTime? denNgay, string trangThai, string hangHK)
        {
            tuNgay ??= DateTime.Now.AddMonths(-1);
            denNgay ??= DateTime.Now;

            ViewData["TuNgay"] = tuNgay.Value.ToString("yyyy-MM-dd");
            ViewData["DenNgay"] = denNgay.Value.ToString("yyyy-MM-dd");
            ViewData["TrangThai"] = trangThai;
            ViewData["HangHK"] = hangHK;

            ViewData["TrangThaiList"] = new List<SelectListItem>
    {
        new SelectListItem { Text = "Hoạt động", Value = "HoatDong" },
        new SelectListItem { Text = "Đã khóa", Value = "BiKhoa" }
    };

            var hangHKList = await _context.MayBay.Select(m => m.TenHangHK).Distinct().ToListAsync();
            ViewData["HangHangKhong"] = hangHKList.Select(h => new SelectListItem { Text = h, Value = h }).ToList();

            // Lấy người dùng KHách hàng theo trạng thái (mặc định là KhachHang)
            var nguoiDungQuery = _context.NguoiDung.Include(n => n.TaiKhoan)
                                     .Where(n => n.TaiKhoan.VaiTro == "KhachHang").AsQueryable();

            if (!string.IsNullOrEmpty(trangThai))
                nguoiDungQuery = nguoiDungQuery.Where(n => n.TaiKhoan.TrangThaiTK == trangThai);

            var tongNguoiDung = await nguoiDungQuery.CountAsync();

            var thangHienTai = DateTime.Now.Month;
            var namHienTai = DateTime.Now.Year;

            var soNguoiDungMoiThangNay = await nguoiDungQuery
                .Where(n => n.TaiKhoan.NgayTao.HasValue &&
                            n.TaiKhoan.NgayTao.Value.Month == thangHienTai &&
                            n.TaiKhoan.NgayTao.Value.Year == namHienTai)
                .CountAsync();

            ViewData["TongNguoiDung"] = tongNguoiDung;
            ViewData["SoNguoiDungMoi"] = soNguoiDungMoiThangNay;

            // Thống kê chuyến bay theo hãng hàng không (nếu chọn)
            var chuyenBayQuery = _context.ChuyenBay.Include(c => c.MayBay).AsQueryable();
            if (!string.IsNullOrEmpty(hangHK))
                chuyenBayQuery = chuyenBayQuery.Where(c => c.MayBay.TenHangHK == hangHK);

            var tongChuyenBay = await chuyenBayQuery.CountAsync();

            // Vé máy bay theo khoảng thời gian lọc và hãng hàng không
            var veQuery = _context.VeMayBay
                .Include(v => v.ChuyenBay).ThenInclude(c => c.MayBay)
                .Include(v => v.ChuyenBay).ThenInclude(c => c.SanBayDiInfo)
                .Where(v => v.ThoiGianDat >= tuNgay && v.ThoiGianDat <= denNgay);

            if (!string.IsNullOrEmpty(hangHK))
                veQuery = veQuery.Where(v => v.ChuyenBay.MayBay.TenHangHK == hangHK);

            var tongVe = await veQuery.CountAsync();

            // Tổng doanh thu
            var tongDoanhThu = await _context.ThanhToan
                .Where(t => t.ThoiGianGiaoDich >= tuNgay && t.ThoiGianGiaoDich <= denNgay)
                .SumAsync(t => (decimal?)t.SoTien) ?? 0;

            // Doanh thu theo tháng
            var doanhThuTheoThang = await _context.ThanhToan
                .Where(t => t.ThoiGianGiaoDich >= tuNgay && t.ThoiGianGiaoDich <= denNgay)
                .GroupBy(t => new { t.ThoiGianGiaoDich.Year, t.ThoiGianGiaoDich.Month })
                .Select(g => new
                {
                    Thang = g.Key.Month + "/" + g.Key.Year,
                    Tong = g.Sum(t => t.SoTien)
                }).ToListAsync();
            ViewData["DataDoanhThuTheoThang"] = JsonSerializer.Serialize(doanhThuTheoThang);

            // Top 5 sân bay đặt vé
            var datTheoSanBay = await veQuery
                .GroupBy(v => v.ChuyenBay.SanBayDiInfo.TenSanBay)
                .Select(g => new { SanBay = g.Key, SoLuong = g.Count() })
                .OrderByDescending(x => x.SoLuong)
                .Take(5)
                .ToListAsync();
            ViewData["DataDatTheoSanBay"] = JsonSerializer.Serialize(datTheoSanBay);

            // Vé huỷ theo tháng
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

            // Doanh thu theo phương thức thanh toán
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

            // Bảng chi tiết tổng hợp
            var bangChiTiet = doanhThuTheoPTTT.Select(dt => new
            {
                Thang = dt.Thang,
                DoanhThu = dt.ChiTiet.Sum(x => x.Tong),
                SoVe = _context.VeMayBay.Count(v => v.ThoiGianDat.Year.ToString() + "/" + v.ThoiGianDat.Month.ToString() == dt.Thang),
                NguoiDungMoi = _context.NguoiDung.Count(n => n.TaiKhoan.NgayTao.HasValue && (n.TaiKhoan.NgayTao.Value.Year.ToString() + "/" + n.TaiKhoan.NgayTao.Value.Month.ToString()) == dt.Thang),
                TyLeHuy = tyLeHuy.FirstOrDefault(x => x.Thang == dt.Thang)?.TyLe ?? 0,
                PhuongThucThanhToan = dt.ChiTiet
            }).ToList();

            ViewData["BangChiTiet"] = bangChiTiet;

            // --- Mới thêm phần ---

            // Top 5 khách hàng mua vé nhiều nhất
            var topKhachHang = await _context.VeMayBay
                .Include(v => v.NguoiDung)
                .Where(v => v.ThoiGianDat >= tuNgay && v.ThoiGianDat <= denNgay)
                .GroupBy(v => new { v.NguoiDung.TenDangNhap, v.NguoiDung.HoTen })
                .Select(g => new
                {
                    TenDangNhap = g.Key.TenDangNhap,
                    HoTen = g.Key.HoTen,
                    SoVeDaDat = g.Count()
                })
                .OrderByDescending(x => x.SoVeDaDat)
                .Take(5)
                .ToListAsync();
            ViewData["TopKhachHang"] = topKhachHang;

            // Số vé theo trạng thái vé theo tháng
            var veTheoTrangThaiTheoThang = await _context.VeMayBay
                .Where(v => v.ThoiGianDat >= tuNgay && v.ThoiGianDat <= denNgay)
                .GroupBy(v => new { v.ThoiGianDat.Year, v.ThoiGianDat.Month, v.TrangThaiVe })
                .Select(g => new
                {
                    Nam = g.Key.Year,
                    Thang = g.Key.Month,
                    TrangThaiVe = g.Key.TrangThaiVe,
                    SoLuong = g.Count()
                })
                .ToListAsync();

            var veTheoTrangThaiTheoThangGrouped = veTheoTrangThaiTheoThang
                .GroupBy(x => $"{x.Thang}/{x.Nam}")
                .Select(g => new
                {
                    Thang = g.Key,
                    SoLuongTheoTrangThai = g.ToDictionary(x => x.TrangThaiVe, x => x.SoLuong)
                })
                .ToList();

            ViewData["VeTheoTrangThaiTheoThang"] = JsonSerializer.Serialize(veTheoTrangThaiTheoThangGrouped);

            // --- Kết thúc phần mới ---

            ViewData["TongChuyenBay"] = tongChuyenBay;
            ViewData["TongVe"] = tongVe;
            ViewData["TongDoanhThu"] = tongDoanhThu;

            return View();
        }

        public IActionResult XuatExcel(DateTime? tuNgay, DateTime? denNgay)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;


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

            // Load dữ liệu vào worksheet, có header
            worksheet.Cells.LoadFromCollection(chiTiet, true);

            int totalRows = chiTiet.Count + 1; // +1 vì có header
            int totalCols = 4; // Số cột dữ liệu

            // Định dạng header
            using (var headerRange = worksheet.Cells[1, 1, 1, totalCols])
            {
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                headerRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                headerRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            }

            // Căn chỉnh dữ liệu cột Tháng căn giữa
            worksheet.Cells[2, 1, totalRows, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

            // Căn chỉnh số liệu cột Doanh Thu, Đã đặt, Người dùng mới căn phải
            worksheet.Cells[2, 2, totalRows, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

            // Định dạng tiền tệ cho cột DoanhThu (cột 2)
            worksheet.Cells[2, 2, totalRows, 2].Style.Numberformat.Format = "#,##0 ₫";

            // Tự động điều chỉnh độ rộng cột theo dữ liệu
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // Thêm AutoFilter cho vùng dữ liệu (bao gồm header)
            worksheet.Cells[1, 1, totalRows, totalCols].AutoFilter = true;

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
