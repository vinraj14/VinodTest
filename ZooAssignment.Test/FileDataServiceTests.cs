using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using ZooAssignment.Interfaces;
using ZooAssignment.Services;

namespace ZooAssignment.Test
{
    public class FileDataServiceTests
    {

        private readonly FileDataService _sut;
        private readonly Mock<IFileReaderService> _filereaderMock = new Mock<IFileReaderService>();
        private readonly IConfiguration _config;
        private readonly Mock<ILogger<FileDataService>> _logger = new Mock<ILogger<FileDataService>>();

        public FileDataServiceTests()
        {
            _sut = new FileDataService(_logger.Object, _config
                , _filereaderMock.Object);

        }

        [Fact]
        public void GetRates_ReturnPrices_WhenFileExists()
        {

            //Arrange

            //Act
            var a = _sut.GetPrice();
            //Assert

            Assert.NotNull(a);

        }

        [Fact]
        public void GetRates_ShowError_WhenOneOfTheRateIsMissing()
        {

        }


        [Fact]
        public void GetRates_ShowError_WhenOneOfTheRateIsZero()
        {

        }


        [Fact]
        public void GetRates_ReturnRateCount()
        {

        }

        [Fact]
        public void GetAnimals_ReturnsAnimals_WhenFileExistwithProperData()
        {

        }

        [Fact]
        public void GetAnimals_ShowError_WhenFileExistwithIncorrectData()
        {

        }

        [Fact]
        public void GetAnimals_CheckIfMeatPercentageExists()
        {

        }

        [Fact]
        public void GetAnimals_CheckIfFoodTypeExists()
        {

        }


        [Fact]
        public void GetAnimals_CheckIfFoodPercentageExists_ForBothType()
        {

        }

        [Fact]
        public void GetZooContent_ReturnsAnimalsWithWeight_WhenFileExistWithCorrectData()
        {

        }

        [Fact]
        public void GetZooContent_ShowError_WhenFileExistWithInCorrectData()
        {

        }

        [Fact]
        public void GetZooContent_CheckIfAnimalWeightExists()
        {

        }


        [Fact]
        public void GetZooContent_CheckIfAnimalNameExists()
        {

        }

    }
}
