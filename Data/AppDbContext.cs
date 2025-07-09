using AkaryakitOtomasyonu.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AkaryakitOtomasyonu.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Tank> Tanks { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Fueling> Fuelings { get; set; }
        public DbSet<FuelPrice> FuelPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 🆕 Tank → Station ilişkisi
            builder.Entity<Tank>()
                .HasOne(t => t.Station)
                .WithMany()
                .HasForeignKey(t => t.StationId)
                .OnDelete(DeleteBehavior.SetNull);

            // Fueling → Tank ilişkisi
            builder.Entity<Fueling>()
                .HasOne(f => f.Tank)
                .WithMany()
                .HasForeignKey(f => f.TankId)
                .OnDelete(DeleteBehavior.Restrict);

            // Fueling → User ilişkisi
            builder.Entity<Fueling>()
                .HasOne(f => f.PerformedByUser)
                .WithMany()
                .HasForeignKey(f => f.PerformedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Fueling>()
               .HasOne(f => f.Tank)
               .WithMany()
               .HasForeignKey(f => f.TankId)
               .OnDelete(DeleteBehavior.Cascade); // Bu satır silerken bağlı kayıtları da siler
        }
    }
}
