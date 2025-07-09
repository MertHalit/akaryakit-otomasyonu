using AkaryakitOtomasyonu.Data;
using AkaryakitOtomasyonu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AkaryakitOtomasyonu.Controllers
{
    [Authorize]
    public class FuelingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public FuelingController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Fueling/Create
        [Authorize(Roles = "Otomasyoncu")]
        public IActionResult Create()
        {
            ViewBag.Tanks = _context.Tanks.ToList();
            return View();
        }

        // POST: Fueling/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Otomasyoncu")]
        public async Task<IActionResult> Create(int tankId, double amount)
        {
            ViewBag.Tanks = _context.Tanks.ToList();

            if (amount <= 0)
            {
                ModelState.AddModelError("", "Geçerli bir miktar girin.");
                return View();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "Kullanıcı bilgisi alınamadı.");
                return View();
            }

            var tank = await _context.Tanks.FirstOrDefaultAsync(t => t.Id == tankId);
            if (tank == null)
            {
                ModelState.AddModelError("", "Tank bulunamadı.");
                return View();
            }

            if (tank.CurrentLevel + amount > tank.Capacity)
            {
                ModelState.AddModelError("", "Bu miktar tankın kapasitesini aşar.");
                return View();
            }

            tank.CurrentLevel += amount;
            _context.Tanks.Update(tank);
            var stationName = await _context.Stations

            .Where(s => s.Id == tank.StationId)
           .Select(s => s.Name)
           .FirstOrDefaultAsync();


            var fueling = new Fueling
            {
                TankId = tank.Id,
                Amount = amount,
                Timestamp = DateTime.UtcNow,
                PerformedByUserId = userId,
                StationName = stationName
            };

            _context.Fuelings.Add(fueling);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Yakıt dolumu başarıyla kaydedildi.";
            return RedirectToAction("Index");
        }

        // GET: Fueling/Report
        [Authorize(Roles = "Admin,Otomasyoncu")]
        [HttpGet]
        public async Task<IActionResult> Report(DateTime? startDate, DateTime? endDate, int? tankId)
        {
            var fuelings = _context.Fuelings
                .Include(f => f.Tank)
                .Include(f => f.PerformedByUser)
                .AsQueryable();

            if (startDate.HasValue)
            {
                var startUtc = DateTime.SpecifyKind(startDate.Value.Date, DateTimeKind.Local).ToUniversalTime();
                fuelings = fuelings.Where(f => f.Timestamp >= startUtc);
            }

            if (endDate.HasValue)
            {
                var endOfDay = endDate.Value.Date.AddDays(1).AddSeconds(-1);
                var endUtc = DateTime.SpecifyKind(endOfDay, DateTimeKind.Local).ToUniversalTime();
                fuelings = fuelings.Where(f => f.Timestamp <= endUtc);
            }

            if (tankId.HasValue)
            {
                fuelings = fuelings.Where(f => f.TankId == tankId.Value);
            }

            var fuelingList = await fuelings.OrderByDescending(f => f.Timestamp).ToListAsync();
            var totalAmount = fuelingList.Sum(f => f.Amount);
            var tanks = await _context.Tanks.ToListAsync();

            ViewBag.TotalAmount = totalAmount;
            ViewBag.Tanks = tanks;

            return View(fuelingList);
        }

        // GET: Fueling/History
        [Authorize(Roles = "Admin,Otomasyoncu")]
        [HttpGet]
        public async Task<IActionResult> History(DateTime? startDate, DateTime? endDate)
        {
            var fuelings = _context.Fuelings
                .Include(f => f.Tank)
                .Include(f => f.PerformedByUser)
                .AsQueryable();

            if (startDate.HasValue)
            {
                var startUtc = DateTime.SpecifyKind(startDate.Value.Date, DateTimeKind.Local).ToUniversalTime();
                fuelings = fuelings.Where(f => f.Timestamp >= startUtc);
            }

            if (endDate.HasValue)
            {
                var endOfDay = endDate.Value.Date.AddDays(1).AddSeconds(-1);
                var endUtc = DateTime.SpecifyKind(endOfDay, DateTimeKind.Local).ToUniversalTime();
                fuelings = fuelings.Where(f => f.Timestamp <= endUtc);
            }

            var fuelingList = await fuelings.OrderByDescending(f => f.Timestamp).ToListAsync();
            return View(fuelingList);
        }

        // GET: Fueling/Index
        public async Task<IActionResult> Index()
        {
            IQueryable<Fueling> query;

            if (User.IsInRole("Admin"))
            {
                query = _context.Fuelings
                    .Include(f => f.Tank)
                    .Include(f => f.PerformedByUser)
                    .OrderByDescending(f => f.Timestamp);
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                query = _context.Fuelings
                    .Include(f => f.Tank)
                    .Include(f => f.PerformedByUser)
                    .Where(f => f.PerformedByUserId == userId)
                    .OrderByDescending(f => f.Timestamp);
            }

            var fuelings = await query.ToListAsync();
            return View(fuelings);
        }
        
        // GET: Fueling/LowFuel
        [Authorize(Roles = "Admin,Otomasyoncu")]
        [HttpGet]
        public async Task<IActionResult> LowFuel()
        {
            var tanks = await _context.Tanks
                .Include(t => t.Station) // istasyon adı için
                .Where(t => t.Capacity > 0 && (t.CurrentLevel / t.Capacity) < 0.2)
                .ToListAsync();

            return View(tanks);
        }


        //İstasyon RAPORU
        [Authorize(Roles = "Admin,Otomasyoncu")]
        public IActionResult StationReport()
        {
            var fuelings = _context.Fuelings
                .Include(f => f.Tank)
                .ThenInclude(t => t.Station)
                .ToList();

            var data = fuelings
                .Where(f => f.Tank != null && f.Tank.Station != null)
                .GroupBy(f => f.Tank.Station.Name)
                .Select(g => new
                {
                    StationName = g.Key,
                    TotalFuel = g.Sum(f => f.Amount)
                })
                .ToList();

            ViewBag.StationFuelData = JsonConvert.SerializeObject(data);
            return View();

        }


        }
}