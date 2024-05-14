using TaskManager.Service;
using TaskManager.Service.Database;
using TaskManager.Service.Database.Sqlite;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddAuthorization();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddControllers();
services.AddDbContext<TaskManagerContext, SqliteTaskManagerContext>();

var app = builder.Build();
DatabaseInitializer.Seed(app.Services);

// IF app.Environment.IsDevelopment()
app.UseSwagger();
app.UseSwaggerUI();
// ENDIF
//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
