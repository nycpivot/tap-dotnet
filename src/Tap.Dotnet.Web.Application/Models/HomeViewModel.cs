namespace Tap.Dotnet.Web.Application.Models
{
    public class HomeViewModel
    {
        public IList<FavoriteViewModel> Favorites { get; set; } = new List<FavoriteViewModel>();
    }
}
