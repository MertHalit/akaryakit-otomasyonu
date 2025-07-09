using AkaryakitOtomasyonu.Models;
using System;
using System.Threading.Tasks;

namespace AkaryakitOtomasyonu.Services
{
    public class FuelPriceService
    {
        public async Task<FuelPrice> GetPricesAsync(string city)
        {
            decimal gasoline = 0, diesel = 0, lpg = 0;

            switch (city.ToLower())
            {
                case "İstanbul":
                    gasoline = 51.21m;
                    diesel = 52.40m;
                    lpg = 26.06m;
                    break;
                case "Ankara":
                    gasoline = 51.89m;
                    diesel = 53.27m;
                    lpg = 25.96m;
                    break;
                case "İzmir":
                    gasoline = 52.22m;
                    diesel = 53.60m;
                    lpg = 25.91m;
                    break;
                
                case "Diyarbakir":
                    gasoline = 50.80m;
                    diesel = 52.75m;
                    lpg = 25.40m;
                    break;
                default:
                    gasoline = 50.00m;
                    diesel = 50.00m;
                    lpg = 25.00m;
                    break;
            }

            return await Task.FromResult(new FuelPrice
            {
                City = city,
                Gasoline = gasoline,
                Diesel = diesel,
                LPG = lpg,
                RetrievedAt = DateTime.UtcNow
            });
        }
    }
}
