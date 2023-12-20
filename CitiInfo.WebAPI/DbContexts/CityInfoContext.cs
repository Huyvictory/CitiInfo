using CitiInfo.WebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CitiInfo.WebAPI.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData
                (
                new City("New York City") { Id = 1, Description = "The one with that big park." },
                new City("Ho chi minh") { Id = 2, Description = "Large city of Viet nam" },
                new City("Paris") { Id = 3, Description = "The one with that big tower" }
                );

            modelBuilder.Entity<PointOfInterest>().HasData
                (
                new PointOfInterest("Central Park") { Id = 1, CityId = 1, Description = "The most visited park in US" },
                new PointOfInterest("Empire State Building") { Id = 2, CityId = 2, Description = "A skyscraper in US" },
                new PointOfInterest("Cathedral") { Id = 3, CityId = 3, Description = "US" }
                );
            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("connectionString");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
