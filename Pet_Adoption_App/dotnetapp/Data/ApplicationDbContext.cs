using Microsoft.EntityFrameworkCore;
using dotnetapp.Models;

namespace dotnetapp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetAdopter> PetAdopters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define the relationship between Pet and PetAdopter classes - one-to-many
            modelBuilder.Entity<Pet>()
                .HasMany(p => p.PetAdopters)
                .WithOne(pa => pa.Pet)
                .HasForeignKey(pa => pa.PetID);

            // Seed data for Pets
            modelBuilder.Entity<Pet>().HasData(
                new Pet { PetID = 1, Name = "Buddy", Type = "Dog", Age = 3, Availability = true },
                new Pet { PetID = 2, Name = "Mittens", Type = "Cat", Age = 2, Availability = true },
                new Pet { PetID = 3, Name = "Tweety", Type = "Bird", Age = 1, Availability = true },
                new Pet { PetID = 4, Name = "Rocky", Type = "Hamster", Age = 1, Availability = true },
                new Pet { PetID = 5, Name = "Whiskers", Type = "Rabbit", Age = 2, Availability = true }
            );
        }
    }
}
