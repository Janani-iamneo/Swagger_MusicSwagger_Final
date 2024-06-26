using Microsoft.EntityFrameworkCore;
using dotnetapp.Models;

namespace dotnetapp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define the relationship between Vehicle and Booking classes - one-to-many
            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.Bookings)
                .WithOne(b => b.Vehicle)
                .HasForeignKey(b => b.VehicleID);

            base.OnModelCreating(modelBuilder);

            // Seed data for Vehicles
            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle { VehicleID = 1, Make = "Tesla", Model = "Model S", Year = 2024, Availability = true },
                new Vehicle { VehicleID = 2, Make = "Ford", Model = "Mustang", Year = 2024, Availability = true },
                new Vehicle { VehicleID = 3, Make = "Chevrolet", Model = "Camaro", Year = 2024, Availability = false },
                new Vehicle { VehicleID = 4, Make = "BMW", Model = "M3", Year = 2022, Availability = true },
                new Vehicle { VehicleID = 5, Make = "Audi", Model = "A4", Year = 2023, Availability = true }
            );
        }
    }
}
