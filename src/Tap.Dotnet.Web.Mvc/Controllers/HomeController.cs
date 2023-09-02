using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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

        public IActionResult Index()
        {
            var homeViewModel = this.weatherApplication.GetDefaultCriteria();

            try
            {
                var forecast = this.weatherApplication.GetForecast(homeViewModel.ZipCode);

                if (forecast != null && forecast.Count == 5)
                {
                    homeViewModel.WeatherForecast.Add(forecast[0]);
                    homeViewModel.WeatherForecast.Add(forecast[1]);
                    homeViewModel.WeatherForecast.Add(forecast[2]);
                    homeViewModel.WeatherForecast.Add(forecast[3]);
                    homeViewModel.WeatherForecast.Add(forecast[4]);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Index", ex.StackTrace ?? ex.Message);
            }

            return View(homeViewModel);
        }

        [HttpPost]
        public ActionResult Search(HomeViewModel model)
        {
            if (ModelState.IsValid)
            {
                //TODO: SubscribeUser(model.Email);
            }

            return View("Index", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
