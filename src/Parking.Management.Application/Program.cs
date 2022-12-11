using Serilog;
using Serilog.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Parking.Management.Application;

public class Program
{
    public static void Main(string[] args)
    {
        ConfigureSerilogWithElasticSearch();
        CreateHostBuilder(args).Build().Run();
    }

    private static void ConfigureSerilogWithElasticSearch()
    {
        string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("Environment", environment)
            .ReadFrom.Configuration(configuration)
            .WriteTo.Console()
            .CreateLogger();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                webBuilder.UseWebRoot("wwwroot");
                webBuilder.UseStartup<Startup>();
            })
            .UseSerilog();
}