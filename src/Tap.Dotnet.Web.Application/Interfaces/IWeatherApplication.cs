using Tap.Dotnet.Web.Application.Models;

namespace Tap.Dotnet.Web.Application.Interfaces
{
    public interface IWeatherApplication
    {
        HomeViewModel GetFavorites();
        WeatherInfoViewModel SaveFavorite(string zipCode);
        WeatherInfoViewModel GetWeather(string zipCode);
    }
}
