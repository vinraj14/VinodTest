using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooAssignment.Interfaces;
using ZooAssignment.Constant;
using System.Globalization;
using System.IO;
using System.Reflection;
using ZooAssignment.Model;

namespace ZooAssignment.Services
{
    public class ExpensesCalculationService : IExpensesCalculationService
    {
        private readonly ILogger<ExpensesCalculationService> _log;
        private readonly IConfiguration _config;
        private readonly IFileDataService _fileDataService;

        public ExpensesCalculationService(ILogger<ExpensesCalculationService> log, IConfiguration config, IFileDataService fileDataService)
        {
            _log = log;
            _config = config;
            _fileDataService = fileDataService;
        }

        public List<Price> GetPrice()
        {

            string textPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _config.GetValue<string>("PriceTextPath"));
            var prices = _fileDataService.GetPrice(textPath);

            if (prices.Count == 0)
            {
                Console.WriteLine("Price details not availalbe");
                Environment.Exit(0);
            }

            return prices;
        }

        public List<Animals> GetAnimals()
        {

            string csvPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _config.GetValue<string>("AnimalCSVPath"));

            var animals = _fileDataService.GetAnimals(csvPath);
            if (animals.Count == 0)
            {
                Console.WriteLine("Animal details not availalbe");
                Environment.Exit(0);
            }

            return animals;
        }

        public List<ZooContent> GetZooContents() {

            string xmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _config.GetValue<string>("ZooContentPath"));

            var zooContents = _fileDataService.GetZooContent(xmlPath);
            if (zooContents.Count == 0)
            {
                Console.WriteLine("Zoo content details not availalbe");
                Environment.Exit(0);
            }

            return zooContents;
        }

        public decimal GetTotalCost()
        {
            _log.LogInformation("Entered into total cost service");
                

            var prices = GetPrice();

            var animals = GetAnimals();

            var zooContents = GetZooContents();


            var totalWeights = zooContents.GroupBy(a => a.Type).Select(a => new { TotalWeight = a.Sum(b => b.Weight), Name = a.Key }).ToList();

            var allanimalsdiet = (from animalweight in totalWeights
                                  join animaldiet in animals on animalweight.Name equals animaldiet.Animal
                                  select new
                                  {
                                      animaldiet.Animal,
                                      animaldiet.FoodPercentage,
                                      animaldiet.FoodType,
                                      meatweight = animaldiet.FoodType.ToLower() == FoodTypes.meat.ToString() ? animalweight.TotalWeight * decimal.Parse(animaldiet.EatingPercentage) : (animaldiet.FoodType.ToLower() == FoodTypes.both.ToString() ? ((animalweight.TotalWeight * decimal.Parse(animaldiet.EatingPercentage)) * decimal.Parse(animaldiet.FoodPercentage, CultureInfo.InvariantCulture) / 100) : 0),
                                      fruitweight = animaldiet.FoodType.ToLower() == FoodTypes.fruit.ToString() ? animalweight.TotalWeight * decimal.Parse(animaldiet.EatingPercentage) : (animaldiet.FoodType.ToLower() == FoodTypes.both.ToString() ? ((animalweight.TotalWeight * decimal.Parse(animaldiet.EatingPercentage)) * (100 - decimal.Parse(animaldiet.FoodPercentage, CultureInfo.InvariantCulture)) / 100) : 0)

                                  }).ToList();

            var meatcost = allanimalsdiet.ToList().Select(c => c.meatweight).Sum() * (prices.Where(a => a.FoodType.ToLower() == FoodTypes.meat.ToString()).Select(c => c.Rate).FirstOrDefault());
            var fruitcost = allanimalsdiet.ToList().Select(c => c.fruitweight).Sum() * (prices.Where(a => a.FoodType.ToLower() == FoodTypes.fruit.ToString()).Select(c => c.Rate).FirstOrDefault());
            var totalcost = meatcost + fruitcost;

            _log.LogInformation("Total cost : {totalcost}", totalcost);

            _log.LogInformation("exit from total cost service");

            return totalcost;
        }
    }
}
