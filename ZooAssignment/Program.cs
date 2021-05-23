using Serilog;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ZooAssignment.Interfaces;
using ZooAssignment.Services;

namespace ZooAssignment
{
    partial class Program
    {
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration().
                ReadFrom.Configuration(builder.Build()).
                Enrich.FromLogContext().
                WriteTo.Console().
                CreateLogger();

            Log.Logger.Information("Application Starting");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddScoped<IGreetingService, GreetingService>();
                    services.AddScoped<IFileReaderService, FileReaderService>();
                    services.AddScoped<IFileDataService, FileDataService>();
                    services.AddScoped<IExpensesCalculationService, ExpensesCalculationService>();

                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<ExpensesCalculationService>(host.Services);
            svc.GetTotalCost();

        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{ Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }

    }

}
