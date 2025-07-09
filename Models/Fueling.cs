using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AkaryakitOtomasyonu.Models
{
    public class Fueling
    {
        public int Id { get; set; }

        [Required]
        public int TankId { get; set; }
        public Tank Tank { get; set; }

        [Required]
        public string PerformedByUserId { get; set; }
        public User PerformedByUser { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [Required]
        [Range(0.01, 100000, ErrorMessage = "Geçerli bir miktar giriniz.")]
        public double Amount { get; set; }

        public string? StationName { get; set; }
    }
}
