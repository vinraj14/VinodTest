
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Reflection;
using Xunit;
using ZooAssignment.Interfaces;
using ZooAssignment.Services;

namespace ZooAssignment.Test
{
    public class FileReaderServiceTests
    {
        private readonly FileReaderService _sut;

        private readonly Mock<IFileReaderService> _filereaderMock = new Mock<IFileReaderService>();
        private readonly Mock<IConfiguration> _config = new Mock<IConfiguration>();
        private readonly Mock<ILogger<FileReaderService>> _logger = new Mock<ILogger<FileReaderService>>();
        public FileReaderServiceTests()
        {
            _sut = new FileReaderService(_logger.Object, _config.Object);
        }
        [Fact]
        public void Text_Fails_whenFileExistsButNoDataInfile()
        {


        }

        [Fact]
        public void CSV_Fails_whenFileExistsButNoDataInfile()
        {


        }

        [Fact]
        public void XML_Fails_whenFileExistsButNoDataInfile()
        {
            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _config.Object.GetValue<string>("PriceTextPathEmpty"));
            //Act
            var a = _sut.ReadTextFile(path);
            //Assert

            Assert.NotNull(a);
        }

        [Fact]
        public void XML_NoFileFoundError_whenFileDoesNotExists()
        {


        }

        [Fact]
        public void CSV_NoFileFoundError_whenFileDoesNotExists()
        {


        }

        [Fact]
        public void Text_NoFileFoundError_whenFileDoesNotExists()
        {


        }
    }
}
