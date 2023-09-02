using App.Metrics;
using App.Metrics.Reporting.Wavefront.Builder;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Net;
using Tap.Dotnet.Common.Interfaces;
using Wavefront.SDK.CSharp.Common;
using Wavefront.SDK.CSharp.Common.Application;

namespace Tap.Dotnet.Api.Weather.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ForecastController : ControllerBase
    {
        private readonly IApiHelper apiHelper;
        private readonly ILogger<ForecastController> logger;

        public ForecastController(IApiHelper apiHelper, ILogger<ForecastController> logger)
        {
            this.apiHelper = apiHelper;
            this.logger = logger;
        }

        [HttpGet]
        [Route("{zipCode}")]
        public IEnumerable<WeatherForecast> Get(string zipCode)
        {
            var forecast = new List<WeatherForecast>();

            var start = DateTimeUtils.UnixTimeMilliseconds(DateTime.UtcNow);

            using (var handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                };

                using (var httpClient = new HttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri(this.apiHelper.WeatherBitUrl);

                    var key = this.apiHelper.WeatherBitKey;

                    var response = httpClient.GetAsync($"forecast/daily?postal_code={zipCode}&key={key}").Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var content = response.Content.ReadAsStringAsync().Result;
                        forecast = JsonConvert.DeserializeObject<List<WeatherForecast>>(content);
                    }
                }
            }

            //var forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    //Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();

            var min = Convert.ToDouble(forecast.Min(t => t.TemperatureC));
            var max = Convert.ToDouble(forecast.Max(t => t.TemperatureC));
            var tags = new Dictionary<string, string>();

            tags.Add("DeploymentType", "Environment");

            // save as storage in wavefront
            this.apiHelper.WavefrontSender.SendMetric("MinimumRandomForecast", min,
                DateTimeUtils.UnixTimeMilliseconds(DateTime.UtcNow), "tap-dotnet-core-api-weather-env", tags);
            this.apiHelper.WavefrontSender.SendMetric("MaximumRandomForecast", max,
                DateTimeUtils.UnixTimeMilliseconds(DateTime.UtcNow), "tap-dotnet-core-api-weather-env", tags);

            // report metrics
            var applicationTags = new ApplicationTags.Builder("tap-dotnet-core-api-weather-env", "random-forecast-controller").Build();

            var metricsBuilder = new MetricsBuilder();

            metricsBuilder.Report.ToWavefront(
              options =>
              {
                  options.WavefrontSender = this.apiHelper.WavefrontSender;
                  options.Source = "tap-dotnet-core-api-weather-env"; // optional
                  options.WavefrontHistogram.ReportMinuteDistribution = true; // optional
                  options.ApplicationTags = applicationTags;
              });

            var end = DateTimeUtils.UnixTimeMilliseconds(DateTime.UtcNow);

            var traceHeader = this.Request.Headers["X-TraceId"];

            this.apiHelper.WavefrontSender.SendSpan(
                "GetRandomWeatherForecast", start, end, "RandomForecastController",
                new Guid(traceHeader[0]), Guid.NewGuid(),
                ImmutableList.Create(new Guid("82dd7b10-3d65-4a03-9226-24ff106b5041")), null,
                ImmutableList.Create(
                    new KeyValuePair<string, string>("application", "tap-dotnet-core-api-weather-claim"),
                    new KeyValuePair<string, string>("service", "RandomForecastController"),
                    new KeyValuePair<string, string>("http.method", "GET")), null);

            return forecast;
        }
    }
}
