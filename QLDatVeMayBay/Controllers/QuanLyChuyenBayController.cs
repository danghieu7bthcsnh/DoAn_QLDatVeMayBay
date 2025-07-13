using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLDatVeMayBay.Data;
using QLDatVeMayBay.Models;

namespace QLDatVeMayBay.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuanLyChuyenBayController : Controller
    {
        private readonly QLDatVeMayBayContext _context;

        public QuanLyChuyenBayController(QLDatVeMayBayContext context)
        {
            _context = context;
        }

        // GET: /QuanLyChuyenBay/Index
        private const int PageSize = 10;

        public async Task<IActionResult> Index(string tinhTrang, int? sanBayDi, int? sanBayDen, int? idMayBay, int page = 1)
        {
            var query = _context.ChuyenBay
                .Include(c => c.MayBay)
                .Include(c => c.SanBayDiInfo)
                .Include(c => c.SanBayDenInfo)
                .AsQueryable();

            if (!string.IsNullOrEmpty(tinhTrang))
            {
                query = query.Where(cb => cb.TinhTrang == tinhTrang);
            }

            if (sanBayDi.HasValue)
            {
                query = query.Where(cb => cb.SanBayDi == sanBayDi.Value);
            }

            if (sanBayDen.HasValue)
            {
                query = query.Where(cb => cb.SanBayDen == sanBayDen.Value);
            }

            if (idMayBay.HasValue)
            {
                query = query.Where(cb => cb.IDMayBay == idMayBay.Value);
            }

            int totalItems = await query.CountAsync();
            var chuyenBayList = await query
                .OrderByDescending(cb => cb.GioCatCanh)
                .Skip((page - 1) * 10)
                .Take(10)
                .ToListAsync();

            // Dữ liệu dropdown
            ViewBag.TinhTrang = tinhTrang;
            ViewBag.SanBayDi = sanBayDi;
            ViewBag.SanBayDen = sanBayDen;
            ViewBag.IDMayBay = idMayBay;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / 10);

            ViewBag.DanhSachSanBay = await _context.SanBay.ToListAsync();
            ViewBag.DanhSachMayBay = await _context.MayBay.ToListAsync();

            return View(chuyenBayList);
        }


        // GET: /QuanLyChuyenBay/Create
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        // POST: /QuanLyChuyenBay/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChuyenBay chuyenBay)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chuyenBay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            LoadDropdowns();
            return View(chuyenBay);
        }

        // GET: /QuanLyChuyenBay/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var chuyenBay = await _context.ChuyenBay.FindAsync(id);
            if (chuyenBay == null) return NotFound();

            LoadDropdowns();
            return View(chuyenBay);
        }

        // POST: /QuanLyChuyenBay/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ChuyenBay chuyenBay)
        {
            if (ModelState.IsValid)
            {
                _context.Update(chuyenBay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (chuyenBay.SanBayDi == chuyenBay.SanBayDen)
            {
                ModelState.AddModelError("", "Sân bay đi và đến không được trùng nhau.");
            }
            if (chuyenBay.GioHaCanh <= chuyenBay.GioCatCanh)
            {
                ModelState.AddModelError("", "Giờ hạ cánh phải sau giờ cất cánh.");
            }
            LoadDropdowns();
            return View(chuyenBay);
        }

        // GET: /QuanLyChuyenBay/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var chuyenBay = await _context.ChuyenBay
                .Include(cb => cb.MayBay)
               .Include(cb => cb.SanBayDiInfo)
                .Include(cb => cb.SanBayDenInfo)
                .FirstOrDefaultAsync(cb => cb.IDChuyenBay == id);

            if (chuyenBay == null) return NotFound();

            return View(chuyenBay);
        }

        // POST: /QuanLyChuyenBay/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chuyenBay = await _context.ChuyenBay.FindAsync(id);
            if (chuyenBay == null) return NotFound();

            _context.ChuyenBay.Remove(chuyenBay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ChiTiet(int id)
        {
            var chuyenBay = await _context.ChuyenBay
                .Include(cb => cb.MayBay)
                .Include(cb => cb.SanBayDiInfo)
                .Include(cb => cb.SanBayDenInfo)
                .FirstOrDefaultAsync(cb => cb.IDChuyenBay == id);

            if (chuyenBay == null) return NotFound();

            return View(chuyenBay);
        }

        private void LoadDropdowns()
        {
            ViewBag.MayBayList = new SelectList(_context.MayBay, "IDMayBay", "TenHangHK");
            ViewBag.SanBayList = new SelectList(_context.SanBay, "IDSanBay", "TenSanBay");
        }

    }
}
