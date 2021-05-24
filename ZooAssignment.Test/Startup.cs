using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using ZooAssignment.Interfaces;
using ZooAssignment.Services;

namespace ZooAssignment.Test
{

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                       .AddJsonFile($"appsettings.{environment}.json", optional: true)
                       .AddEnvironmentVariables()
                       .Build();           

            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<IFileReaderService, FileReaderService>();
            services.AddScoped<IFileDataService, FileDataService>();
            services.AddScoped<IExpensesCalculationService, ExpensesCalculationService>();          

        }

    }
}
