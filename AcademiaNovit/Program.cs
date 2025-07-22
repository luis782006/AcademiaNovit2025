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

#endregion

#region Leer cadena de conexión

string? dbConnectionFile = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING_FILE");
string connectionString;

if (!string.IsNullOrEmpty(dbConnectionFile))
{
    Console.WriteLine($"[INFO] Variable de entorno DB_CONNECTION_STRING_FILE detectada: {dbConnectionFile}");

    if (File.Exists(dbConnectionFile))
    {
        connectionString = File.ReadAllText(dbConnectionFile).Trim();
        Console.WriteLine($"[INFO] Cadena de conexión leída correctamente desde secreto: {connectionString}");
    }
    else
    {
        Console.WriteLine($"[ERROR] No se encontró el archivo secreto en: {dbConnectionFile}");
        throw new FileNotFoundException($"Archivo secreto no encontrado: {dbConnectionFile}");
    }
}
else
{
    Console.WriteLine("[INFO] No se encontró la variable DB_CONNECTION_STRING_FILE, usando appsettings.json");
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

#endregion

#region Configuración de PostgreSQL

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

#region Endpoint keep-alive

app.MapGet("/keep-alive", () => new
{
    status = "alive",
    timestamp = DateTime.UtcNow
});

#endregion

app.Run();

public partial class Program { }
