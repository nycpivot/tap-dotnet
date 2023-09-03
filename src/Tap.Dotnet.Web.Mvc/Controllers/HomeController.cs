using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection.Emit;
using Tap.Dotnet.Common.Interfaces;
using Tap.Dotnet.Web.Application.Interfaces;
using Tap.Dotnet.Web.Application.Models;
using Tap.Dotnet.Web.Mvc.Models;

namespace Tap.Dotnet.Web.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherApplication weatherApplication;
        private readonly IApiHelper apiHelper;
        private readonly ILogger<HomeController> logger;

        public HomeController(
            IWeatherApplication weatherApplication,
            IApiHelper apiHelper, ILogger<HomeController> logger)
        {
            this.weatherApplication = weatherApplication;
            this.apiHelper = apiHelper;
            this.logger = logger;
        }

        public IActionResult Index(WeatherInfoViewModel model)
        {
            var weatherInfoViewModel = new WeatherInfoViewModel();

            try
            {
                weatherInfoViewModel = this.weatherApplication.GetWeather(model.ZipCode);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Index", ex.StackTrace ?? ex.Message);
            }

            return View(weatherInfoViewModel);
        }

        [HttpPost]
        public ActionResult Search(WeatherInfoViewModel model)
        {
            var weatherInfoViewModel = new WeatherInfoViewModel();

            if (ModelState.IsValid)
            {
                weatherInfoViewModel = this.weatherApplication.GetWeather(model.ZipCode);
            }

            return View("Index", weatherInfoViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
