using Microsoft.EntityFrameworkCore;
using dotnetapp.Models;

namespace dotnetapp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<PartyHall> PartyHalls { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define the relationship between PartyHall and Booking classes - one-to-many
            modelBuilder.Entity<PartyHall>()
                .HasMany(p => p.Bookings)
                .WithOne(b => b.PartyHall)
                .HasForeignKey(b => b.PartyHallID);

            base.OnModelCreating(modelBuilder);

            // Seed data for PartyHalls
            modelBuilder.Entity<PartyHall>().HasData(
                new PartyHall { PartyHallID = 1, Name = "Elegant Banquet Hall", Capacity = 100, Availability = true },
                new PartyHall { PartyHallID = 2, Name = "Cozy Party Room", Capacity = 50, Availability = true },
                new PartyHall { PartyHallID = 3, Name = "Grand Celebration Hall", Capacity = 200, Availability = true },
                new PartyHall { PartyHallID = 4, Name = "Lavish Ballroom", Capacity = 150, Availability = true },
                new PartyHall { PartyHallID = 5, Name = "Rustic Barn Venue", Capacity = 80, Availability = true }
            );
        }
    }
}
