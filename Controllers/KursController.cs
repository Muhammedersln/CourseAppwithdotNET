using CourseApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.Controllers
{
    public class KursController : Controller
    {
        private readonly DataContext _context;
        public KursController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var kurslar = await _context.Kurslar.ToListAsync();
            return View(kurslar);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Kurs model)
        {
            _context.Kurslar.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var kurs = await _context.Kurslar
            .Include(x => x.KursKayitlari)
            .ThenInclude(x => x.Ogrenci)
            .FirstOrDefaultAsync(x => x.KursId == id);
            return View(kurs);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Kurs model)
        {
            _context.Kurslar.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var kurs = await _context.Kurslar.FindAsync(id);
            if (kurs == null)
            {
                return NotFound();
            }
            return View(kurs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var ogrenci = await _context.Kurslar.FindAsync(id);
            if (ogrenci == null)
            {
                return NotFound();
            }
            _context.Kurslar.Remove(ogrenci);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}