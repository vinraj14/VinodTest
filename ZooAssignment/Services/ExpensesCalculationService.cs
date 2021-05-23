using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooAssignment.Interfaces;
using ZooAssignment.Constant;
using System.Globalization;

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



        public decimal GetTotalCost()
        {
            _log.LogInformation("entered into total cost");

            var prices = _fileDataService.GetPrice();
            var animals = _fileDataService.GetAnimals();
            var zooContents = _fileDataService.GetZooContent();

            var totalWeights = zooContents.GroupBy(a => a.Type).Select(a => new { TotalWeight = a.Sum(b => b.Weight), Name = a.Key }).ToList();

            var allanimalsdiet = (from animalweight in totalWeights
                                  join animaldiet in animals on animalweight.Name equals animaldiet.Animal
                                  select new
                                  {
                                      animaldiet.Animal,
                                      animaldiet.FoodPercentage,
                                      animaldiet.FoodType,
                                      meatweight = animaldiet.FoodType.ToLower() == FoodTypes.meat.ToString() ? animalweight.TotalWeight * animaldiet.EatingPercentage : (animaldiet.FoodType.ToLower() == FoodTypes.both.ToString() ? ((animalweight.TotalWeight * animaldiet.EatingPercentage) * decimal.Parse(animaldiet.FoodPercentage, CultureInfo.InvariantCulture) / 100) : 0),
                                      fruitweight = animaldiet.FoodType.ToLower() == FoodTypes.fruit.ToString() ? animalweight.TotalWeight * animaldiet.EatingPercentage : (animaldiet.FoodType.ToLower() == FoodTypes.both.ToString() ? ((animalweight.TotalWeight * animaldiet.EatingPercentage) * (100 - decimal.Parse(animaldiet.FoodPercentage, CultureInfo.InvariantCulture)) / 100) : 0)

                                  }).ToList();

            var meatcost = allanimalsdiet.ToList().Select(c => c.meatweight).Sum() * (prices.Where(a => a.FoodType.ToLower() == FoodTypes.meat.ToString()).Select(c => c.Rate).FirstOrDefault());
            var fruitcost = allanimalsdiet.ToList().Select(c => c.fruitweight).Sum() * (prices.Where(a => a.FoodType.ToLower() == FoodTypes.fruit.ToString()).Select(c => c.Rate).FirstOrDefault());
            var totalcost = meatcost + fruitcost;

            return totalcost;
        }
    }
}
