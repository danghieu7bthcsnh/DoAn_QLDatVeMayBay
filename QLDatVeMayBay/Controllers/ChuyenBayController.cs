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
            ViewBag.SanBayDi = new SelectList(_context.SanBay, "IDSanBay", "TenSanBay", model.SanBayDi);
            ViewBag.SanBayDen = new SelectList(_context.SanBay, "IDSanBay", "TenSanBay", model.SanBayDen);

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
            ViewBag.LoaiVe = model.LoaiVe;
            ViewBag.HangGhe = model.HangGhe;
            ViewBag.KetQua = danhSach;
            return View(model);
        }

    }

}
