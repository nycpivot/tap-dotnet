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

        public IActionResult Index(HomeViewModel model)
        {
            var homeViewModel = this.weatherApplication.GetForecast(model.WeatherInfo.ZipCode);

            return View(homeViewModel);
        }

        [HttpPost]
        public ActionResult Search(HomeViewModel model)
        {
            var homeViewModel = new HomeViewModel();

            if (ModelState.IsValid)
            {
                homeViewModel = this.weatherApplication.GetForecast(model.WeatherInfo.ZipCode);
            }

            return View("Index", homeViewModel);
        }

        [HttpPost]
        public ActionResult Save(HomeViewModel model)
        {
            if (ModelState.IsValid)
            {
                this.weatherApplication.SaveFavorite(model.WeatherInfo.ZipCode);
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
