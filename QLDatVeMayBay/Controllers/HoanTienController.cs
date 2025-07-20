using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLDatVeMayBay.Data;
using QLDatVeMayBay.Models;

namespace QLDatVeMayBay.Controllers
{
    public class HoanTienController : Controller
    {
        private readonly QLDatVeMayBayContext _context;

        public HoanTienController(QLDatVeMayBayContext context)
        {
            _context = context;
        }

        // GET: /HoanTiens
        public IActionResult Index()
        {
            var dsHoan = _context.HoanTien
                .Include(ht => ht.ThanhToan)
                .ThenInclude(tt => tt.VeMayBay)
                .OrderByDescending(ht => ht.NgayHoanTien)
                .ToList();

            return View(dsHoan);
        }
    }
}
