namespace Tap.Dotnet.Web.Application.Models
{
    public class HomeViewModel
    {
        public string ZipCode { get; set; } = String.Empty;

        public IList<WeatherForecastViewModel> WeatherForecast { get; set; } = new List<WeatherForecastViewModel>();
    }
}
