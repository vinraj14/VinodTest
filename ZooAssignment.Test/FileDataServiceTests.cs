using System.IO;
using System.Reflection;

using Xunit;
using Microsoft.Extensions.Logging;
using ZooAssignment.Interfaces;
using ZooAssignment.Services;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace ZooAssignment.Test
{
    public class FileDataServiceTests
    {

        private readonly IFileDataService _fileDataService;
        private readonly IConfiguration _configuration;

        public FileDataServiceTests(IFileDataService fileDataService, IConfiguration configuration)
        {
            _fileDataService = fileDataService;
            _configuration = configuration;
        }

        [Fact]
        public void GetRates_ReturnPrices_WhenFileExists()
        {
            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("PriceTextPath"));

            //Act
            var returnValue = _fileDataService.GetPrice(path);

            //Assert
            Assert.NotNull(returnValue);

        }

        [Fact]
        public void GetRates_ShowError_WhenOneOfTheRateIsMissing()
        {
            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("PriceTextPath"));

            //Act
            var returnValue = _fileDataService.GetPrice(path);

            //Assert
            Assert.NotEqual("2", returnValue.Count.ToString());
        }

        [Fact]
        public void GetRates_ShowError_WhenOneOfTheRateIsZero()
        {
            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("PriceTextPath"));

            //Act
            var returnValue = _fileDataService.GetPrice(path);

            //Assert
            Assert.NotEqual("0", returnValue[0].Rate.ToString());
        }

        [Fact]
        public void GetRates_ReturnRateCount()
        {
            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("PriceTextPath"));

            //Act
            var returnValue = _fileDataService.GetPrice(path);

            //Assert
            Assert.Equal("2", returnValue.Count.ToString());
        }


        [Fact]
        public void GetAnimals_ReturnsAnimals_WhenFileExistwithProperData()
        {
            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("AnimalCSVPath"));

            //Act
            var returnValue = _fileDataService.GetAnimals(path);

            //Assert
            Assert.NotNull(returnValue);
        }

        [Fact]
        public void GetAnimals_ShowError_WhenFileExistwithIncorrectData()
        {
            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("AnimalCSVPathWrongData"));

            //Act
            var returnValue = _fileDataService.GetAnimals(path);

            //Assert
            Assert.Equal("0", returnValue.Count.ToString());
        }
             

        [Fact]
        public void GetZooContent_ReturnsAnimalsWithWeight_WhenFileExistWithCorrectData()
        {
            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("ZooContentPath"));

            //Act
            var returnValue = _fileDataService.GetZooContent(path);

            //Assert
            Assert.NotNull(returnValue);
        }

        [Fact]
        public void GetZooContent_ShowError_WhenFileExistWithInCorrectData()
        {
            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("ZooContentPathEmpty"));

            //Act
            var returnValue = _fileDataService.GetZooContent(path);

            //Assert
            Assert.Null(returnValue);
        }     
      

    }
}
