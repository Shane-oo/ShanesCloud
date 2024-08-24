using Carter;
using ShanesStorage.Api;

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

builder.Services.AddMediatRServices();
builder.Services.AddCarter();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.MapGet("/ping", () => "pong");


app.MapCarter(); // Maps all Minimal Api Endpoints that inherit ICarterModule/CarterModule

app.Run();
