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

builder.Services.AddDataContext(builder.Configuration.GetConnectionString("Shanes-db"));

builder.Services.AddUserIdentity();

builder.Services.AddStorageAccountServices(appSettings.StorageAccountSettings, builder.Environment.IsDevelopment());
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


app.MapGet("/", () => "WHATS UP!");
app.MapGet("/ping", () => "pong");


app.MapCarter(); // Maps all Minimal Api Endpoints that inherit ICarterModule/CarterModule

app.Run();
