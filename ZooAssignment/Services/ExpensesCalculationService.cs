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
        private readonly ILogger<ExpensesCalculationService> _logger;
        private readonly IFileDataService _fileDataService;

        public ExpensesCalculationService(ILogger<ExpensesCalculationService> logger, IFileDataService fileDataService)
        {
            _logger = logger;
            _fileDataService = fileDataService;
        }

        public List<Price> GetPrice()
        {
            Console.WriteLine("Please enter path of price text file with file name and extension");
            string txtPath = Console.ReadLine();

            var prices = _fileDataService.GetPrice(txtPath.Trim());

            if (prices.Count == 0)
            {
                Console.WriteLine("Press 1 to continue or press any key to exit from application");
                ConsoleKeyInfo action = Console.ReadKey();

                if (action.KeyChar == '1')
                {
                    Console.WriteLine();
                    prices = GetPrice();
                }
                else
                    Environment.Exit(0);
            }


            return prices;
        }

        public List<Animals> GetAnimals()
        {
            Console.WriteLine("Please enter path of animal csv file with file name and extension");
            string csvPath = Console.ReadLine();

            var animals = _fileDataService.GetAnimals(csvPath.Trim());
            if (animals.Count == 0)
            {
                Console.WriteLine("Press 1 to continue or press any key to exit from application");
                ConsoleKeyInfo action = Console.ReadKey();
                if (action.KeyChar == '1')
                {
                    Console.WriteLine();
                    animals = GetAnimals();
                }
                else
                    Environment.Exit(0);
            }

            return animals;
        }

        public List<ZooContent> GetZooContents()
        {
            Console.WriteLine("Please enter path of zoo content xml file with file name and extension");
            string xmlPath = Console.ReadLine();

            var zooContents = _fileDataService.GetZooContent(xmlPath.Trim());
            if (zooContents == null)
            {
                Console.WriteLine("Press 1 to continue or press any key to exit from application");
                ConsoleKeyInfo action = Console.ReadKey();
                if (action.KeyChar == '1')
                {
                    Console.WriteLine();
                    zooContents = GetZooContents();
                }
                else
                    Environment.Exit(0);
            }

            return zooContents;
        }

        public decimal GetTotalCost()
        {
            var prices = GetPrice();
            var animals = GetAnimals();
            var zooContents = GetZooContents();
            return CalcualteTotalCost(prices, animals, zooContents);
        }

        public decimal CalcualteTotalCost(List<Price> prices, List<Animals> animals, List<ZooContent> zooContents)
        {
            decimal totalcost = 0;

            try
            {
                _logger.LogInformation("Entered into total cost service");

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
                totalcost = meatcost + fruitcost;

                _logger.LogInformation("Total cost : {totalcost}", totalcost);

                _logger.LogInformation("exit from total cost service");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong while calculating expenses");
                _logger.LogError(ex.Message);
            }


            return totalcost;
        }
    }
}
