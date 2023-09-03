using Tap.Dotnet.Web.Application.Models;

namespace Tap.Dotnet.Web.Application.Interfaces
{
    public interface IWeatherApplication
    {
        WeatherInfoViewModel GetWeather(string zipCode);
    }
}
