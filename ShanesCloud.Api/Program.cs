using Azure.Monitor.OpenTelemetry.AspNetCore;
using Carter;
using ShanesCloud.Api;

var builder = WebApplication.CreateBuilder(args);


// Settings
var appSettings = builder.Configuration
                         .GetSection("AppSettings")
                         .Get<AppSettings>();

builder.Services
       .AddOptions<AppSettings>()
       .BindConfiguration("AppSettings")
       .ValidateDataAnnotations()
       .ValidateOnStart();


builder.Services.AddStorageAccountServices(appSettings.StorageAccountSettings, builder.Environment);

builder.Services.AddFluentValidators();
builder.Services.AddMediatRServices();
builder.Services.AddCarter();


if (!builder.Environment.IsDevelopment())
{
    // Applications Insights
    builder.Services.AddOpenTelemetry()
           .UseAzureMonitor();
}


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandling>();


app.MapGet("/", () => "Hello World Application Everything worked!");
app.MapGet("/ping", () => "pong");


app.MapCarter(); // Maps all Minimal Api Endpoints that inherit ICarterModule/CarterModule

app.Run();
