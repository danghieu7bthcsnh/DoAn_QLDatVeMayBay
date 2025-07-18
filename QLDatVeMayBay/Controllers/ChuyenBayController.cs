using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLDatVeMayBay.Data;
using QLDatVeMayBay.Models;

namespace QLDatVeMayBay.Controllers
{
    public class ChuyenBayController : Controller
    {
        private readonly QLDatVeMayBayContext _context;

        public ChuyenBayController(QLDatVeMayBayContext context)
        {
            _context = context;
        }

        public IActionResult TimKiem()
        {
            ViewBag.SanBayDi = new SelectList(_context.SanBay.ToList(), "IDSanBay", "TenSanBay");
            ViewBag.SanBayDen = new SelectList(_context.SanBay.ToList(), "IDSanBay", "TenSanBay");
            return View();
        }

        [HttpPost]
        public IActionResult KetQuaTimKiem(TimKiemChuyenBay model)
        {
            var danhSach = _context.ChuyenBay
                .Include(cb => cb.MayBay)
                .ThenInclude(cb => cb.LoaiMayBay)
                .Include(cb => cb.SanBayDiInfo)
                .Include(cb => cb.SanBayDenInfo)
                .Where(cb =>
                    cb.SanBayDi == model.SanBayDi &&
                    cb.SanBayDen == model.SanBayDen &&
                    cb.GioCatCanh.Date == model.NgayDi.Date)
                .ToList();

            ViewBag.ThongTin = model;
            return View(danhSach);
        }
        [HttpPost]
        public IActionResult TimKiem(TimKiemChuyenBay model)
        {
            // Load dropdown lại để tránh mất dữ liệu
            ViewBag.SanBayDi = new SelectList(_context.SanBay, "IDSanBay", "TenSanBay", model.SanBayDi);
            ViewBag.SanBayDen = new SelectList(_context.SanBay, "IDSanBay", "TenSanBay", model.SanBayDen);
            ViewBag.LoaiVe = model.LoaiVe;
            ViewBag.HangGhe = model.HangGhe;

            // ======= 1. Kiểm tra để trống toàn bộ =======
            if (model.SanBayDi == null && model.SanBayDen == null && model.NgayDi == null && model.LoaiVe == null)
            {
                ViewBag.Loi = "Vui lòng nhập đầy đủ thông tin";
                return View(model);
            }

            // ======= 2. Kiểm tra sân bay đi và đến trùng nhau =======
            if (model.SanBayDi == model.SanBayDen && model.SanBayDi != null)
            {
                ViewBag.Loi = "Không tìm thấy chuyến bay phù hợp";
                return View(model);
            }

            // ======= 3. Kiểm tra ngày đi trong quá khứ =======
            if (model.NgayDi < DateTime.Today)
            {
                ViewBag.Loi = "Ngày đi không hợp lệ";
                return View(model);
            }

            // ======= 5. Tìm chuyến bay phù hợp =======
            var danhSach = _context.ChuyenBay
                .Include(cb => cb.MayBay)
                .Include(cb => cb.SanBayDiInfo)
                .Include(cb => cb.SanBayDenInfo)
                .Where(cb =>
                    cb.SanBayDi == model.SanBayDi &&
                    cb.SanBayDen == model.SanBayDen &&
                    cb.GioCatCanh.Date == model.NgayDi.Date
                )
                .ToList();

            if (!danhSach.Any())
            {
                ViewBag.Loi = "Không tìm thấy chuyến bay phù hợp";
                return View(model);
            }

            ViewBag.KetQua = danhSach;
            return View(model);
        }


    }

}
