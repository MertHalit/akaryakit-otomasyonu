using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AkaryakitOtomasyonu.Data;
using AkaryakitOtomasyonu.Models;

namespace AkaryakitOtomasyonu.Controllers
{
    public class StationsController : Controller
    {
        private readonly AppDbContext _context;

        public StationsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var stations = await _context.Stations.ToListAsync();
            return View(stations);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var station = await _context.Stations.FirstOrDefaultAsync(m => m.Id == id);
            if (station == null) return NotFound();

            return View(station);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Station station)
        {
            if (ModelState.IsValid)
            {
                _context.Add(station);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(station);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var station = await _context.Stations.FindAsync(id);
            if (station == null) return NotFound();

            return View(station);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Station station)
        {
            if (id != station.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(station);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await StationExists(station.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(station);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var station = await _context.Stations.FirstOrDefaultAsync(m => m.Id == id);
            if (station == null) return NotFound();

            return View(station);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var station = await _context.Stations.FindAsync(id);
            if (station != null)
            {
                _context.Stations.Remove(station);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> StationExists(int id)
        {
            return await _context.Stations.AnyAsync(e => e.Id == id);
        }
    }
}
