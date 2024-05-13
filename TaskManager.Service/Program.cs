using TaskManager.Service;
using TaskManager.Service.Database;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddAuthorization()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddTaskManagerContext(builder.Configuration);

var app = builder.Build();

//IF app.Environment.IsDevelopment()

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

var summaries = new[]
{
"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (HttpContext httpContext) =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        })
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();