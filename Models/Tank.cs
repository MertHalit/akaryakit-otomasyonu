namespace AkaryakitOtomasyonu.Models
{
    public class Tank
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public double Capacity { get; set; }
        public double CurrentLevel { get; set; }
        public string? ControlledBy { get; set; }

        // 🆕 Tank → Station ilişkisi
        public int? StationId { get; set; }
        public Station? Station { get; set; }
    }
}
