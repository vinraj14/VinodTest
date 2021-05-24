using Xunit;
using System.IO;
using System.Reflection;
using ZooAssignment.Interfaces;
using ZooAssignment.Model;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ZooAssignment.Test
{
    public class FileReaderServiceTests
    {
        private readonly IFileReaderService _fileReaderService;     
        private readonly IConfiguration _configuration; 

        public FileReaderServiceTests(IFileReaderService fileReaderService, IConfiguration configuration)
        {
            _fileReaderService = fileReaderService;           
            _configuration = configuration;           
        }

        //When file does't have data

        [Fact]
        public void Text_Fails_whenFileExistsButNoDataInfile()
        {            
            //Arrange   
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("PriceTextPathEmpty"));  

            //Act            
            var returnValue = _fileReaderService.ReadTextFile(path);

            //Assert  
            Assert.Equal("0", returnValue.Count.ToString());

        }

        [Fact]
        public void CSV_Fails_whenFileExistsButNoDataInfile()
        {
            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("AnimalCSVPathEmpty"));

            //Act
            var returnValue = _fileReaderService.ReadCSVFile<Animals>(path);

            //Assert  
            Assert.Equal("0", returnValue.Count.ToString());
        }

        [Fact]
        public void XML_Fails_whenFileExistsButNoDataInfile()
        {
            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("ZooContentPathEmpty"));

            //Act
            var returnValue = _fileReaderService.ReadXMLFile(path);

            //Assert
            Assert.Null(returnValue);
        }


        //When file does't exists

        [Fact]
        public void Text_NoFileFoundError_whenFileDoesNotExists()
        {

            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("PriceTextPathNotThere"));

            //Act
            var returnValue = _fileReaderService.ReadTextFile(path);

            //Assert  
            Assert.Equal("0", returnValue.Count.ToString());
        }

        [Fact]
        public void CSV_NoFileFoundError_whenFileDoesNotExists()
        {

            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("AnimalCSVPathNotThere"));

            //Act
            var returnValue = _fileReaderService.ReadCSVFile<Animals>(path);

            //Assert  
            Assert.Equal("0", returnValue.Count.ToString());
        }

        [Fact]
        public void XML_NoFileFoundError_whenFileDoesNotExists()
        {
            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("ZooContentPathNotThere"));

            //Act
            var returnValue = _fileReaderService.ReadXMLFile(path);

            //Assert
            Assert.Null(returnValue);

        }


        //When file have data

        [Fact]
        public void Text_Pass_DataExists_FileExists()
        {

            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("PriceTextPath"));

            //Act
            var returnValue = _fileReaderService.ReadTextFile(path);

            //Assert  
            Assert.Equal("2", returnValue.Count.ToString());
        }

        [Fact]
        public void CSV_Pass_DataExists_FileExists()
        {

            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("AnimalCSVPath"));

            //Act
            var returnValue = _fileReaderService.ReadCSVFile<Animals>(path);

            //Assert  
            Assert.Equal("6", returnValue.Count.ToString());
        }

        [Fact]
        public void XML_Pass_DataExists_FileExists()
        {
            //Arrange
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _configuration.GetValue<string>("ZooContentPath"));

            //Act
            var returnValue = _fileReaderService.ReadXMLFile(path);

            //Assert
            Assert.NotNull(returnValue);

        }
    }
}
