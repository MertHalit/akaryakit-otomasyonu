using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AkaryakitOtomasyonu.Data;
using AkaryakitOtomasyonu.Models;
using Microsoft.EntityFrameworkCore;

namespace AkaryakitOtomasyonu.Controllers
{
    public class TankController : Controller
    {
        private readonly AppDbContext _context;

        public TankController(AppDbContext context)
        {
            _context = context;
        }

        // Listeleme
        public IActionResult Index()
        {
            var tanks = _context.Tanks.Include(t => t.Station).ToList();
            return View(tanks);
        }

        // Tank oluşturma formu (GET)
        public IActionResult Create()
        {
            ViewBag.Stations = new SelectList(_context.Stations.ToList(), "Id", "Name");
            return View();
        }

        // Tank oluşturma işlemi (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tank tank)
        {
            if (ModelState.IsValid)
            {
                _context.Tanks.Add(tank);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Stations = new SelectList(_context.Stations.ToList(), "Id", "Name", tank.StationId);
            return View(tank);
        }

        // Tank düzenleme formu (GET)
        public IActionResult Edit(int id)
        {
            var tank = _context.Tanks.Find(id);
            if (tank == null)
                return NotFound();

            var stations = _context.Stations.ToList();
            ViewBag.Stations = new SelectList(stations, "Id", "Name", tank.StationId);

            return View(tank);
        }

        // Tank düzenleme işlemi (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Tank tank)
        {
            if (ModelState.IsValid)
            {
                _context.Tanks.Update(tank);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            var stations = _context.Stations.ToList();
            ViewBag.Stations = new SelectList(stations, "Id", "Name", tank.StationId);

            return View(tank);
        }

        // Tank silme formu (GET)
        public IActionResult Delete(int id)
        {
            var tank = _context.Tanks.Include(t => t.Station).FirstOrDefault(t => t.Id == id);
            if (tank == null)
                return NotFound();

            return View(tank);
        }

        // POST: Tank/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var tank = _context.Tanks.Find(id);
            if (tank != null)
            {
                _context.Tanks.Remove(tank);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
