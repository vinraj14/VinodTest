
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Text;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

// DI , Serilog, Settings

namespace ZooAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //GetPrice();
            //GetAnimals();

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
                    services.AddTransient<IGreetingService, GreetingService>();

                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<GreetingService>(host.Services);
            svc.Run();

        }

        public static void GetPrice()
        {

            try
            {
                FileStream fileStream = new FileStream(@"D:\Test Project\ZooAssignment\ZooAssignment\Resource\Price\prices.txt", FileMode.Open);
                //read file line by line using StreamReader
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        //print line
                        Console.WriteLine(line);
                    }

                }
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                // Something went wrong.
                Console.WriteLine("The file could not be read:");
                //print error message
                Console.WriteLine(e.Message);
            }

        }

        public static void GetAnimals()
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(false, ';');
            CsvUserDetailsMapping csvMapper = new CsvUserDetailsMapping();
            CsvParser<Animals> csvParser = new CsvParser<Animals>(csvParserOptions, csvMapper);
            var result = csvParser.ReadFromFile(@"D:\Test Project\ZooAssignment\ZooAssignment\Resource\Animals\animals.csv", Encoding.ASCII).ToList();
            Console.WriteLine("Animal " + "EatingPercentage   " + "FoodType  " + "FoodPercentage");
            foreach (var details in result)
            {
                Console.WriteLine(details.Result.Animal + " " + details.Result.EatingPercentage + " " + details.Result.FoodType + " " + details.Result.FoodPercentage);
            }
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{ Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }

    public class Animals
    {
        public string Animal { get; set; }
        public decimal EatingPercentage { get; set; }
        public string FoodType { get; set; }
        public string FoodPercentage { get; set; }
    }

    class CsvUserDetailsMapping : CsvMapping<Animals>
    {
        public CsvUserDetailsMapping() : base()
        {
            MapProperty(0, x => x.Animal);
            MapProperty(1, x => x.EatingPercentage);
            MapProperty(2, x => x.FoodType);
            MapProperty(3, x => x.FoodPercentage);
        }
    }


}
