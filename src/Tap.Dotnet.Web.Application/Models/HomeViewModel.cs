﻿namespace Tap.Dotnet.Web.Application.Models
{
    public class HomeViewModel
    {
        public WeatherInfoViewModel WeatherInfo { get; set; } = new WeatherInfoViewModel();
        public IList<FavoriteViewModel> Favorites { get; set; } = new List<FavoriteViewModel>();
    }
}
