using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions options): base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Bangladesh",
                    ShortName = "BN"
                },
                new Country
                {
                    Id=2,
                    Name = "India",
                    ShortName = "IND"
                },
                new Country
                {
                    Id=3,
                    Name = "China",
                    ShortName = "CHN"
                }
                );

            builder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Lameridian",
                    Address = "Khilkhet",
                    CountryId = 1,
                    Rating =4.5
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Marbel Residency",
                    Address = "Samajiguda",
                    CountryId = 2,
                    Rating = 4.5
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Wanchu Pande",
                    Address = "China Bridg",
                    CountryId = 3,
                    Rating = 4.5
                }


                );

        }

    public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
    }
}
