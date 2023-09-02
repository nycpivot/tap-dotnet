using Tap.Dotnet.Web.Application.Models;

namespace Tap.Dotnet.Web.Application.Interfaces
{
    public interface IWeatherApplication
    {
        HomeViewModel GetDefaultCriteria();
        IList<WeatherForecastViewModel> GetForecast(string zipCode);
    }
}
