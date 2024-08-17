using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
                {
                    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                };

app.MapGet("/weatherforecast", () =>
                               {
                                   var forecast = Enumerable.Range(1, 5).Select(index =>
                                                                                    new WeatherForecast
                                                                                        (
                                                                                         DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                                                                         Random.Shared.Next(-20, 55),
                                                                                         summaries[Random.Shared.Next(summaries.Length)]
                                                                                        ))
                                                            .ToArray();
                                   return forecast;
                               });

app.MapGet("", () => "Hello Shane, Ya Big boy");

app.MapGet("/test/{id:int}", ([FromRoute] int id) => Results.Ok("You gave me this is: " + id));

app.MapGet("/test", ([AsParameters] MyTestQuery query) => Results.Ok($"You gave me this ID: {query.Id} and Page: {query.Page}"));

app.MapPost("/test", (MyTestPost myTestPost) => Results.Ok(myTestPost));

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class MyTestQuery
{
    public string? Id { get; set; }

    public int? Page { get; set; }
}

public class MyTestPost
{
    public string? Name { get; set; }

    public int? Age { get; set; }
}
