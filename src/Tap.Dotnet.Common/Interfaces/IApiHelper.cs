using StackExchange.Redis;
using Wavefront.SDK.CSharp.Common;

namespace Tap.Dotnet.Common.Interfaces
{
    public interface IApiHelper
    {
        string DefaultZipCode { get; set; }
        string WeatherApiUrl { get; set; }
        IWavefrontSender WavefrontSender { get; set; }
        IDatabase CacheDb { get; set; }
    }
}
