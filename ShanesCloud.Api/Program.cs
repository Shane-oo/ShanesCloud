using Azure.Monitor.OpenTelemetry.AspNetCore;
using Carter;
using OpenIddict.Validation.AspNetCore;
using ShanesCloud.Api;
using ShanesCloud.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsDevelopment())
{
    // Applications Insights
    builder.Services.AddOpenTelemetry()
           .UseAzureMonitor();
}

// Settings
var appSettings = builder.Configuration
                         .GetSection("AppSettings")
                         .Get<AppSettings>();

builder.Services
       .AddOptions<AppSettings>()
       .BindConfiguration("AppSettings")
       .ValidateDataAnnotations()
       .ValidateOnStart();

const string CorsPolicy = "CorsPolicy";
builder.Services.AddCors(o =>
                         {
                             o.AddPolicy(CorsPolicy, p =>
                                                     {
                                                         p.AllowAnyHeader()
                                                          .AllowAnyMethod()
                                                          .AllowCredentials()
                                                          .WithOrigins(appSettings.AllowedClientUrls);
                                                     });
                         });

builder.Services.AddDataContext(builder.Configuration.GetConnectionString("ShanesDb"));

// needed with minimal apis using Authorization used to use .AddControllersWithViews()
builder.Services.AddAuthorizationBuilder()
       .AddPolicy(nameof(Roles.Admin), policy => policy
                                           .RequireRole(nameof(Roles.Admin)))
       .AddPolicy(nameof(Roles.User), policy => policy
                                          .RequireRole(nameof(Roles.User)))
       .AddPolicy(nameof(Roles.Guest), policy => policy
                                           .RequireRole(nameof(Roles.Guest)));


builder.Services.AddOpenIddictServer(appSettings, builder.Environment.IsDevelopment());

builder.Services.AddUserIdentity();

builder.Services.AddAuthentication(o =>
                                   {
                                       o.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                       o.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                       o.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                                   });


builder.Services.AddStorageAccountServices(appSettings.StorageAccountSettings, builder.Environment.IsDevelopment());
builder.Services.AddFluentValidators();
builder.Services.AddMediatRServices();
builder.Services.AddCarter();

var app = builder.Build();


if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors(CorsPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandling>();


app.MapGet("/", () => "WHATS UP!");
app.MapGet("/ping", () => "pong");

app.MapCarter(); // Maps all Minimal Api Endpoints that inherit ICarterModule/CarterModule

app.Run();
