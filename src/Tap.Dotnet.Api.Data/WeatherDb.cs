using Microsoft.EntityFrameworkCore;
using Tap.Dotnet.Domain;

namespace Tap.Dotnet.Api.Data
{
    public class WeatherDb : DbContext
    {
        public WeatherDb(DbContextOptions<WeatherDb> options) : base(options)
        {
        
        }
        public DbSet<Favorite> Favorites { get; set; }
    }
}
