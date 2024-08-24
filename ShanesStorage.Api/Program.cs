using Carter;
using ShanesStorage.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatRServices();
builder.Services.AddCarter();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.MapGet("/ping", () => "pong");

/*app.MapGet("/test/{id:int}/{subId:int}", (int id, int subId) => Results.Ok("You gave me this is: " + id + "And thus" + subId));

app.MapGet("/test", ([AsParameters] MyTestQuery query) => Results.Ok($"You gave me this ID: {query.Id} and Page: {query.Page}"));

app.MapPost("/test", (MyTestPost myTestPost) => Results.Ok(myTestPost));*/


app.MapCarter(); // Maps all Minimal Api Endpoints that inherit ICarterModule/CarterModule

app.Run();
