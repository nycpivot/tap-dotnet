using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Net;
using System.Reflection.Emit;
using Tap.Dotnet.Common.Interfaces;
using Tap.Dotnet.Web.Application.Interfaces;
using Tap.Dotnet.Web.Application.Models;

namespace Tap.Dotnet.Web.Application
{
    public class WeatherApplication : IWeatherApplication
    {
        private readonly IApiHelper apiHelper;

        public WeatherApplication(IApiHelper apiHelper)
        {
            this.apiHelper = apiHelper;
        }

        public HomeViewModel GetFavorites()
        {
            var homeViewModel = new HomeViewModel();

            var traceId = Guid.NewGuid();
            var spanId = Guid.NewGuid();

            //this.apiHelper.WavefrontSender.SendSpan(
            //    "Get", 0, 1, "ForecastController", traceId, spanId,
            //    ImmutableList.Create(new Guid("82dd7b10-3d65-4a03-9226-24ff106b5041")), null,
            //    ImmutableList.Create(
            //        new KeyValuePair<string, string>("application", "tap-dotnet-web-mvc"),
            //        new KeyValuePair<string, string>("service", "GetWeather"),
            //        new KeyValuePair<string, string>("http.method", "GET")), null);

            using (var handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                };

                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri(this.apiHelper.WeatherApi);
                    httpClient.DefaultRequestHeaders.Add("X-TraceId", traceId.ToString());

                    var response = httpClient.GetAsync($"favorites").Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;

                        homeViewModel.Favorites = JsonConvert.DeserializeObject<IList<FavoriteViewModel>>(content);
                    }
                }
            }

            return homeViewModel;
        }

        public WeatherInfoViewModel SaveFavorite(string zipCode)
        {
            throw new NotImplementedException();
        }

        public WeatherInfoViewModel GetWeather(string zipCode)
        {
            var weatherInfo = new WeatherInfoViewModel();

            var traceId = Guid.NewGuid();
            var spanId = Guid.NewGuid();

            //this.apiHelper.WavefrontSender.SendSpan(
            //    "Get", 0, 1, "ForecastController", traceId, spanId,
            //    ImmutableList.Create(new Guid("82dd7b10-3d65-4a03-9226-24ff106b5041")), null,
            //    ImmutableList.Create(
            //        new KeyValuePair<string, string>("application", "tap-dotnet-web-mvc"),
            //        new KeyValuePair<string, string>("service", "GetWeather"),
            //        new KeyValuePair<string, string>("http.method", "GET")), null);

            using (var handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                };

                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri(this.apiHelper.WeatherApi);
                    httpClient.DefaultRequestHeaders.Add("X-TraceId", traceId.ToString());

                    var response = httpClient.GetAsync($"forecast/{zipCode}").Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;

                        weatherInfo = JsonConvert.DeserializeObject<WeatherInfoViewModel>(content);
                        weatherInfo.ZipCode = zipCode;
                    }
                }
            }

            return weatherInfo;
        }
    }
}