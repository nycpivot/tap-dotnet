using Microsoft.EntityFrameworkCore;
using Tap.Dotnet.Domain;

namespace Tap.Dotnet.Api.Data.Contexts
{
    public class WeatherDb : DbContext
    {
        public DbSet<Favorite> Favorites { get; set; }
    }
}
