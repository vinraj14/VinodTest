
using Xunit;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

using ZooAssignment.Model;
using ZooAssignment.Services;
using ZooAssignment.Interfaces;



namespace ZooAssignment.Test
{
    public class ExpenseCalculationServiceTests
    {
        private readonly IFileDataService _fileDataService;
        private readonly IExpensesCalculationService _expensesCalculationService;
        private readonly IConfiguration _configuration;

        public ExpenseCalculationServiceTests(IExpensesCalculationService expensesCalculationService, IFileDataService fileDataService, IConfiguration configuration)
        {
            _expensesCalculationService = expensesCalculationService;
            _fileDataService = fileDataService;
            _configuration = configuration;
        }

        [Fact]
        public void CheckIfReturnsTotalcost()
        {

            //Arrange
            string txtPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("PriceTextPath"));
            string csvPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("AnimalCSVPath"));
            string xmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("ZooContentPath"));

            //Act
            var prices = _fileDataService.GetPrice(txtPath);
            var animals = _fileDataService.GetAnimals(csvPath);
            var zooContent = _fileDataService.GetZooContent(xmlPath);
            var totalCost = _expensesCalculationService.CalcualteTotalCost(prices, animals, zooContent);

            //Assert
            Assert.Equal("1609.00896", totalCost.ToString());

        }

    }
}
