using AcademiaNovit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Configuración de Serilog

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Configuration.AddEnvironmentVariables();

#endregion

#region Leer cadena de conexión (secreto o appsettings)

string? dbConnectionFile = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING_FILE");
string connectionString;

if (!string.IsNullOrEmpty(dbConnectionFile) && File.Exists(dbConnectionFile))
{
    //Leer cadena de conexión desde el archivo secreto (docker secrets)
    connectionString = File.ReadAllText(dbConnectionFile);
    Console.WriteLine($"[INFO] Usando cadena de conexión desde secreto: {dbConnectionFile}");
}
else
{
    //Si no existe el secreto, usa appsettings.json (modo local sin Docker)
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    Console.WriteLine("[INFO] Usando cadena de conexión desde appsettings.json");
}

#endregion

#region Configurar conexión Postgres

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

#endregion

builder.Services.AddOpenApi();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.MapOpenApi();
app.MapScalarApiReference();
app.MapControllers();

#region Keep-alive endpoint

app.MapGet("/keep-alive", () => new
{
    status = "alive",
    timestamp = DateTime.UtcNow
});

#endregion

app.Run();

public partial class Program { } // Requerido para tests con WebApplicationFactory.
