using AkaryakitOtomasyonu.Data;
using AkaryakitOtomasyonu.Models;
using AkaryakitOtomasyonu.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AkaryakitOtomasyonu.Controllers
{
    [Authorize]
    public class FuelPriceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly FuelPriceService _fuelService;

        public FuelPriceController(AppDbContext context, FuelPriceService fuelService)
        {
            _context = context;
            _fuelService = fuelService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var prices = await _context.FuelPrices
                .OrderByDescending(p => p.RetrievedAt)
                .ToListAsync();

            var distinctPrices = prices
                .GroupBy(p => p.City.Trim().ToLower())
                .Select(g => g.First())
                .ToList();

            foreach (var item in distinctPrices)
            {
                item.City = char.ToUpper(item.City[0]) + item.City.Substring(1);
            }

            return View(distinctPrices);
        }

        [HttpPost]
        public async Task<IActionResult> Index(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                TempData["Error"] = "Lütfen bir şehir giriniz.";
                return RedirectToAction("Index");
            }

            try
            {
                var price = await _fuelService.GetPricesAsync(city);
                price.RetrievedAt = DateTime.UtcNow;
                _context.FuelPrices.Add(price);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Veri alınamadı: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
