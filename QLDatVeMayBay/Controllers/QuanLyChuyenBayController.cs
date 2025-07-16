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
        private const int PageSize = 10;

        public QuanLyChuyenBayController(QLDatVeMayBayContext context)
        {
            _context = context;
        }

        // GET: /QuanLyChuyenBay/Index
        public async Task<IActionResult> Index(string tinhTrang, int? sanBayDi, int? sanBayDen, int? idMayBay, int page = 1)
        {
            var query = _context.ChuyenBay
                .Include(c => c.MayBay)
                .Include(c => c.SanBayDiInfo)
                .Include(c => c.SanBayDenInfo)
                .AsQueryable();

            // 🔎 Lọc
            if (!string.IsNullOrEmpty(tinhTrang))
                query = query.Where(cb => cb.TinhTrang == tinhTrang);

            if (sanBayDi.HasValue)
                query = query.Where(cb => cb.SanBayDi == sanBayDi.Value);

            if (sanBayDen.HasValue)
                query = query.Where(cb => cb.SanBayDen == sanBayDen.Value);

            if (idMayBay.HasValue)
                query = query.Where(cb => cb.IDMayBay == idMayBay.Value);

            // 📄 Phân trang
            int totalItems = await query.CountAsync();
            var chuyenBayList = await query
                .OrderByDescending(cb => cb.GioCatCanh)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            // 🔽 Danh sách Dropdowns
            var danhSachSanBay = await _context.SanBay.ToListAsync();
            var danhSachMayBay = await _context.MayBay.ToListAsync();

            ViewBag.SanBayDiList = new SelectList(danhSachSanBay, "IDSanBay", "TenSanBay", sanBayDi);
            ViewBag.SanBayDenList = new SelectList(danhSachSanBay, "IDSanBay", "TenSanBay", sanBayDen);
            ViewBag.MayBayList = new SelectList(danhSachMayBay, "IDMayBay", "TenHangHK", idMayBay);

            // ✅ Dropdown tình trạng
            ViewBag.TinhTrangList = new List<SelectListItem>
            {
                new SelectListItem { Text = "🟢 Đang bay", Value = "Đang bay", Selected = tinhTrang == "Đang bay" },
                new SelectListItem { Text = "🟡 Hoãn", Value = "Hoãn", Selected = tinhTrang == "Hoãn" },
                new SelectListItem { Text = "🔴 Hủy", Value = "Hủy", Selected = tinhTrang == "Hủy" }
            };

            // 📦 Thông tin phân trang và giữ lại giá trị filter
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / PageSize);
            ViewBag.TinhTrang = tinhTrang;
            ViewBag.SanBayDi = sanBayDi;
            ViewBag.SanBayDen = sanBayDen;
            ViewBag.IDMayBay = idMayBay;

            return View(chuyenBayList);
        }

        // GET: /QuanLyChuyenBay/Create
        public IActionResult Create()
        {
            LoadDropdowns();
            return View(new ChuyenBay());
        }

        // POST: /QuanLyChuyenBay/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChuyenBay chuyenBay)
        {
            // Validation logic
            if (chuyenBay.SanBayDi == chuyenBay.SanBayDen)
            {
                ModelState.AddModelError("", "Sân bay đi và đến không được trùng nhau.");
            }
            if (chuyenBay.GioHaCanh <= chuyenBay.GioCatCanh)
            {
                ModelState.AddModelError("", "Giờ hạ cánh phải sau giờ cất cánh.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(chuyenBay);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm chuyến bay thành công!";
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
            // Validation logic
            if (chuyenBay.SanBayDi == chuyenBay.SanBayDen)
            {
                ModelState.AddModelError("", "Sân bay đi và đến không được trùng nhau.");
            }
            if (chuyenBay.GioHaCanh <= chuyenBay.GioCatCanh)
            {
                ModelState.AddModelError("", "Giờ hạ cánh phải sau giờ cất cánh.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chuyenBay);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật chuyến bay thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChuyenBayExists(chuyenBay.IDChuyenBay))
                        return NotFound();
                    throw;
                }
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chuyenBay = await _context.ChuyenBay.FindAsync(id);
            if (chuyenBay == null) return NotFound();

            try
            {
                _context.ChuyenBay.Remove(chuyenBay);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Xóa chuyến bay thành công!";
            }
            catch (Exception)
            {
                TempData["Error"] = "Không thể xóa chuyến bay này vì có dữ liệu liên quan.";
            }

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

        private bool ChuyenBayExists(int id)
        {
            return _context.ChuyenBay.Any(e => e.IDChuyenBay == id);
        }
    }
}