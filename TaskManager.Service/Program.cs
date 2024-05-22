using FastEndpoints;
using FastEndpoints.Swagger;
using TaskManager.Service.Database;
using TaskManager.Service.Database.Sqlite;
using TaskManager.Service.Mapper;
using TaskManager.Service.Utils;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services);
var app = builder.Build();
ConfigureWebApplication(app);
app.Run();

IServiceCollection ConfigureServices(IServiceCollection services)
{
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
    app.UseFastEndpoints(x =>
    {
        EndpointsConfigurator.ConfigureRouteVersioning(x.Endpoints);
        x.Endpoints.RoutePrefix = "api";
        x.Versioning.DefaultVersion = 1;
        x.Versioning.Prefix = "v";
        x.Versioning.PrependToRoute = true;
    });
    app.UseSwaggerGen();

    return webApplication;
}
