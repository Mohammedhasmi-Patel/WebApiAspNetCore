
using api.Data;
using api.Extensions;
using Microsoft.EntityFrameworkCore;

try
{
    Console.WriteLine("[STARTUP] Application starting up...");
    Console.Out.Flush();
    var builder = WebApplication.CreateBuilder(args);
    
    Console.WriteLine("[STARTUP] Configuring project services...");
    Console.Out.Flush();
    builder.Services.ConfigureProjectServices(builder.Configuration);
    
    Console.WriteLine("[STARTUP] Building application...");
    Console.Out.Flush();
    var app = builder.Build();

    var port = Environment.GetEnvironmentVariable("PORT");
    if (!string.IsNullOrWhiteSpace(port))
    {
        Console.WriteLine($"[STARTUP] Binding to PORT: {port}");
        app.Urls.Add($"http://*:{port}");
    }
    else
    {
        Console.WriteLine("[STARTUP] No PORT environment variable found, using default port.");
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    Console.WriteLine("[STARTUP] Creating service scope for migrations...");
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        Console.WriteLine("[STARTUP] Attempting to apply database migrations...");
        await context.Database.MigrateAsync();
        Console.WriteLine("[STARTUP] Database migrations successfully applied.");
    }

    app.UseHttpsRedirection();
    app.UseCors("AllowFrontendApp");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    Console.WriteLine("[STARTUP] Starting the web server...");
    app.Run();
}
catch (Exception ex)
{
    string crashLog = $"\n=================================\n" +
                      $"[FATAL CRASH] Error: {ex.Message}\n" +
                      $"[FATAL CRASH] StackTrace: {ex.StackTrace}\n";
                      
    if (ex.InnerException != null)
    {
        crashLog += $"[FATAL CRASH] Inner Exception: {ex.InnerException.Message}\n";
    }
    crashLog += "=================================";

    Console.WriteLine(crashLog);
    Console.Out.Flush();
    
    // Give Vercel's logging daemon 1 second to actually transmit the log before the process violently dies
    System.Threading.Thread.Sleep(1000);
    throw;
}

