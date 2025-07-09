using System;

namespace AkaryakitOtomasyonu.Models
{
    public class FuelPrice
    {
        public int Id { get; set; }

        public string City { get; set; } = null!;   // Şehir adı

        public decimal Gasoline { get; set; }       // Benzin fiyatı
        public decimal Diesel { get; set; }         // Motorin fiyatı
        public decimal LPG { get; set; }            // LPG fiyatı

        public DateTime RetrievedAt { get; set; }   // Verinin çekilme tarihi
    }
}
