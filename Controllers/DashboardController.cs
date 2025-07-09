using AkaryakitOtomasyonu.Data;
using AkaryakitOtomasyonu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AkaryakitOtomasyonu.Controllers
{
    [Authorize(Roles = "Otomasyoncu")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public DashboardController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var toplamDolum = await _context.Fuelings.SumAsync(f => f.Amount);
            var toplamTank = await _context.Tanks.CountAsync();
            var toplamKullanici = _userManager.Users.Count();

            ViewBag.ToplamLitre = toplamDolum;
            ViewBag.ToplamTank = toplamTank;
            ViewBag.AktifKullanici = toplamKullanici;

            // 📊 Tank doluluk oranlarını grafik için hazırla
            var tankChartData = await _context.Tanks
                .Select(t => new
                {
                    name = t.Name,
                    percentage = t.Capacity > 0 ? Math.Round((t.CurrentLevel / t.Capacity) * 100, 2) : 0
                }).ToListAsync();

            ViewBag.TankChartData = tankChartData;

            return View();
        }
    }
}
