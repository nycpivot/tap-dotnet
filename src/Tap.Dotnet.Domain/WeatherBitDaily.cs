namespace Tap.Dotnet.Domain
{
    public class WeatherBitDaily
    {
        public string city_name { get; set; } = String.Empty;
        public string state_code { get; set; } = String.Empty;
        public string country_code { get; set; } = String.Empty;
        public string lat { get; set; } = String.Empty;
        public string lon { get; set; } = String.Empty;
        public string timezone { get; set; } = String.Empty;

        public WeatherBitDescription weather { get; set; } = new WeatherBitDescription();
    }
}
