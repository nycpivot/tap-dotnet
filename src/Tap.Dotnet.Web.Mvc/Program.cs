using Microsoft.CodeAnalysis.CSharp.Syntax;
using StackExchange.Redis;
using System.Diagnostics;
using System.Net.Security;
using System.Reflection;
using Tap.Dotnet.Common;
using Tap.Dotnet.Common.Interfaces;
using Tap.Dotnet.Web.Application;
using Tap.Dotnet.Web.Application.Interfaces;
using Tap.Dotnet.Web.Mvc.Models;
using Wavefront.SDK.CSharp.DirectIngestion;

var builder = WebApplication.CreateBuilder(args);

var serviceBindings = Environment.GetEnvironmentVariable("SERVICE_BINDING_ROOT") ?? String.Empty;

// read environment variables
var weatherApi = Environment.GetEnvironmentVariable("WEATHER_API") ?? String.Empty;

// read secrets from files
var wavefrontUrl = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-api-resource-claim", "host"));
var wavefrontToken = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "wavefront-api-resource-claim", "token"));
var cacheConnection = System.IO.File.ReadAllText(Path.Combine(serviceBindings, "redis-cache-class-claim", "connection"));

var wfSender = new WavefrontDirectIngestionClient.Builder(wavefrontUrl, wavefrontToken).Build();

var redisConnection = ConnectionMultiplexer.Connect(cacheConnection);
//var cacheServer = redisConnection.GetServer(cacheConnection);
var cacheDb = redisConnection.GetDatabase();

builder.Services.AddSingleton<IDatabase>(cacheDb);

var apiHelper = new ApiHelper()
{
    WeatherApi = weatherApi,
    WavefrontSender = wfSender,
    //CacheDb = cacheDb
};

builder.Services.AddSingleton<IApiHelper>(apiHelper);
builder.Services.AddSingleton<IWeatherApplication, WeatherApplication>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

//void LoadCache(IDatabase db)
//{
//    var ass = Assembly.GetExecutingAssembly();

//    var path = $"{ass.GetName().Name}.Data.uszips.csv";
//    var stream = new StreamReader(ass.GetManifestResourceStream(path));

//    var lineNumber = 0;
//    while(!stream.EndOfStream)
//    {
//        if(lineNumber > 0) // skip column headings
//        {
//            var line = stream.ReadLine();
//            var fields = line.Split(",");

//            var zipCode = fields[0];

//            //var location = new LocationViewModel()
//            //{
//            //    ZipCode = fields[0],
//            //    Latitude = fields[1],
//            //    Longitude = fields[2],
//            //    CityName = fields[3],
//            //    StateCode = fields[4],
//            //    StateName = fields[5],
//            //    Population = Convert.ToInt32(fields[8])
//            //};

//            db.SetAdd(new RedisKey(zipCode), new RedisValue(line));
//        }

//        lineNumber++;
//    }
//}
