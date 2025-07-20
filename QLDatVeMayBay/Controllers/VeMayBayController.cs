using iText.Kernel.Geom;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using QLDatVeMayBay.Models.ViewModels;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLDatVeMayBay.Data;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace QLDatVeMayBay.Controllers
{
    [Authorize(Roles = "KhachHang")]
    public class VeMayBayController : Controller
    {
        private readonly QLDatVeMayBayContext _context;

        private const int PageSize = 10;
        public async Task<IActionResult> Index(string tuKhoa, string trangThai, int? idChuyenBay, string hangGhe, DateTime? ngayDat, int page = 1)
        {
            var query = _context.VeMayBay
                .Include(v => v.NguoiDung)
                .Include(v => v.ChuyenBay).ThenInclude(cb => cb.MayBay)
                .Include(v => v.Ghe)
                .AsQueryable();

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                tuKhoa = tuKhoa.ToLower();
                query = query.Where(v => v.NguoiDung.HoTen.ToLower().Contains(tuKhoa) ||
                                         v.NguoiDung.Email.ToLower().Contains(tuKhoa) ||
                                         v.ChuyenBay.IDChuyenBay.ToString().Contains(tuKhoa));
            }

            if (!string.IsNullOrEmpty(trangThai))
            {
                query = query.Where(v => v.TrangThaiVe == trangThai);
            }

            if (idChuyenBay.HasValue)
            {
                query = query.Where(v => v.IDChuyenBay == idChuyenBay);
            }

            if (!string.IsNullOrEmpty(hangGhe))
            {
                query = query.Where(v => v.Ghe.HangGhe == hangGhe);
            }

            if (ngayDat.HasValue)
            {
                var dateOnly = ngayDat.Value.Date;
                query = query.Where(v => v.ThoiGianDat.Date == dateOnly);
            }

            int totalItems = await query.CountAsync();

            var danhSachVe = await query
                .OrderByDescending(v => v.ThoiGianDat)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.TuKhoa = tuKhoa;
            ViewBag.TrangThai = trangThai;
            ViewBag.IDChuyenBay = idChuyenBay;
            ViewBag.HangGhe = hangGhe;
            ViewBag.NgayDat = ngayDat?.ToString("yyyy-MM-dd");
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / PageSize);

            ViewBag.TrangThaiList = new List<SelectListItem>
            {
                new SelectListItem { Text = "✅ Đã thanh toán", Value = "Đã thanh toán" },
                new SelectListItem { Text = "⌛ Chưa thanh toán", Value = "Chưa thanh toán" },
                new SelectListItem { Text = "❌ Đã huỷ", Value = "Đã huỷ" }
            };

            ViewBag.ChuyenBayList = _context.ChuyenBay
                .Include(cb => cb.MayBay)
                .Select(cb => new SelectListItem
                {
                    Value = cb.IDChuyenBay.ToString(),
                    Text = cb.IDChuyenBay + " - " + cb.MayBay.TenHangHK
                }).ToList();

            ViewBag.HangGheList = await _context.GheNgoi
                .Select(g => g.HangGhe)
                .Distinct()
                .ToListAsync();

            return View(danhSachVe);
        }


        public IActionResult ChuyenBayCuaToi()
        {
            var tenDangNhap = User.Identity?.Name;


            var userId = _context.TaiKhoan
            .Include(t => t.NguoiDung)
            .FirstOrDefault(t => t.TenDangNhap == tenDangNhap).NguoiDung.IDNguoiDung;


            var danhSach = _context.VeMayBay
                .Include(v => v.ChuyenBay)
                    .ThenInclude(cb => cb.MayBay)
                .Include(v => v.ChuyenBay.SanBayDiInfo)
                .Include(v => v.ChuyenBay.SanBayDenInfo)
                .Include(v => v.Ghe)
                .Include(v => v.ThanhToan)
                .Where(v => v.IDNguoiDung == userId)
                .Select(v => new ChuyenBayCuaToi
                {
                    IDVe = v.IDVe,
                    MaChuyenBay = "CB" + v.ChuyenBay!.IDChuyenBay,
                    GioCatCanh = v.ChuyenBay.GioCatCanh,
                    GioHaCanh = v.ChuyenBay.GioHaCanh,
                    SanBayDi = v.ChuyenBay.SanBayDiInfo!.TenSanBay,
                    SanBayDen = v.ChuyenBay.SanBayDenInfo!.TenSanBay,
                    TenMayBay = v.ChuyenBay.MayBay!.TenHangHK,
                    HangGhe = v.Ghe!.HangGhe,
                    LoaiVe = v.LoaiVe ?? "Thường",
                    PhuongThucThanhToan = v.ThanhToan != null ? v.ThanhToan.PhuongThuc ?? "Chưa rõ" : "Chưa thanh toán",
                    TrangThaiThanhToan = v.ThanhToan != null ? v.ThanhToan.TrangThaiThanhToan ?? "Chưa thanh toán" : "Chưa thanh toán",
                    TrangThaiVe = v.TrangThaiVe ?? "Chưa rõ",
                    TinhTrangChuyenBay = v.ChuyenBay.TinhTrang ?? "Chưa cập nhật"
                })
                .OrderByDescending(v => v.GioCatCanh)
                .ToList();


            return View(danhSach);
        }




    }
}