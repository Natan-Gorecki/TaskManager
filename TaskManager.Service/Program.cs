using Ardalis.GuardClauses;
using FastEndpoints;
using FastEndpoints.Swagger;
using System.Text.Json.Serialization;
using TaskManager.Service.Database;
using TaskManager.Service.Database.Sqlite;
using TaskManager.Service.Mapper;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder);
var app = builder.Build();
ConfigureWebApplication(app);
app.Run();

IServiceCollection ConfigureServices(WebApplicationBuilder webApplicationBuilder)
{
    var services = builder.Services;

    services.AddCors(corsOptions =>
    {
        corsOptions.AddDefaultPolicy(corsPolicyBuilder =>
        {
            var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
            Guard.Against.Null(allowedOrigins);
            corsPolicyBuilder.WithOrigins(allowedOrigins)
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
    services.AddControllers();
    services.AddAutoMapper(typeof(MappingProfile));
    services.AddAuthorization();
    services.AddFastEndpoints();
    services.SwaggerDocument(x =>
    {
        x.ShortSchemaNames = true;
        x.MinEndpointVersion = 1;
        x.MaxEndpointVersion = 1;
    });
    services.AddDbContext<TaskManagerContext, SqliteTaskManagerContext>();
    return services;
}

IApplicationBuilder ConfigureWebApplication(WebApplication webApplication)
{
    DatabaseInitializer.Seed(app.Services);

    //app.UseAuthorization();
    //app.UseHttpsRedirection();
    app.UseCors();
    app.UseFastEndpoints(x =>
    {
        x.Endpoints.Configurator = ep =>
        {
            ep.AllowAnonymous(); // TODO Enable HTTPS for endpoints
            RouteVersioningConfigurator.Configure(ep);
            ep.PreProcessor<RequestNullabilityValidator>(Order.Before);
        };
        x.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
        x.Endpoints.RoutePrefix = "api";
        x.Versioning.DefaultVersion = 1;
        x.Versioning.Prefix = "v";
        x.Versioning.PrependToRoute = true;
    });
    app.UseSwaggerGen();

    return webApplication;
}
