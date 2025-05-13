namespace WF.Mimetic.UI.Server;

using Microsoft.AspNetCore.Builder;
using Serilog;
using System;
using System.IO;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        Log.Logger = ConfigureLogger(builder);
        Startup startup = new Startup(builder.Configuration, builder.Environment);

        try
        {
            Log.Information("Starting Web Host");
            startup.ConfigureServices(builder.Services);
            WebApplication app = builder.Build();
            startup.Configure(app);
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static ILogger ConfigureLogger(WebApplicationBuilder builder)
    {

        builder.Host.UseSerilog();

        return new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Filter.ByExcluding(evt => 
                evt.Properties.ContainsKey("RequestPath") 
                && evt.Properties["RequestPath"].ToString().Contains("/api/v0/Logs"))
            .CreateLogger();
    }
}