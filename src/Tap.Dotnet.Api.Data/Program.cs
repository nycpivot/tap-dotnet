using Microsoft.EntityFrameworkCore;
using Tap.Dotnet.Api.Data.Contexts;
using Wavefront.SDK.CSharp.Common;
using Wavefront.SDK.CSharp.DirectIngestion;

var builder = WebApplication.CreateBuilder(args);

var serviceBindings = Environment.GetEnvironmentVariable("SERVICE_BINDING_ROOT") ?? String.Empty;

var weatherDbHost = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "weather-db", "host"));
var weatherDbName = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "weather-db", "database"));
var weatherDbUsername = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "weather-db", "username"));
var weatherDbPassword = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "weather-db", "password"));
var wavefrontUrl = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-api", "host"));
var wavefrontToken = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-api", "token"));

// setup postgres database
var weatherDbConnectionString = $"Host={weatherDbHost}; Database={weatherDbName}; Username={weatherDbUsername}; Password={weatherDbPassword};";

builder.Services.AddDbContext<WeatherDb>(options => options.UseNpgsql(weatherDbConnectionString));

// setup monitoring with wavefront
var wfSender = new WavefrontDirectIngestionClient.Builder(wavefrontUrl, wavefrontToken).Build();

builder.Services.AddSingleton<IWavefrontSender>(wfSender);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
