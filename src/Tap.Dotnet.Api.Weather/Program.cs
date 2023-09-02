using Tap.Dotnet.Common.Interfaces;
using Tap.Dotnet.Common;
using Wavefront.SDK.CSharp.DirectIngestion;

var builder = WebApplication.CreateBuilder(args);

var serviceBindings = Environment.GetEnvironmentVariable("SERVICE_BINDING_ROOT") ?? String.Empty;
var weatherBitUrl = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "weather-bit-api", "host"));
var weatherBitKey = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "weather-bit-api", "key"));
var wavefrontUrl = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-api", "host"));
var wavefrontToken = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-api", "token"));

var wfSender = new WavefrontDirectIngestionClient.Builder(wavefrontUrl, wavefrontToken).Build();

var apiHelper = new ApiHelper()
{
    WeatherBitUrl = weatherBitUrl,
    WeatherBitKey = weatherBitKey,
    WavefrontSender = wfSender
};

builder.Services.AddSingleton<IApiHelper>(apiHelper);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
