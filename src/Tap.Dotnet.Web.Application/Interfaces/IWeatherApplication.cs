using Tap.Dotnet.Web.Application.Models;

namespace Tap.Dotnet.Web.Application.Interfaces
{
    public interface IWeatherApplication
    {
        HomeViewModel GetForecast(string zipCode);
        void SaveFavorite(string zipCode);
    }
}
