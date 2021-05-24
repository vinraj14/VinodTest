using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using ZooAssignment.Interfaces;
using ZooAssignment.Model;

namespace ZooAssignment.Services
{
    public class FileDataService : IFileDataService
    {
        private readonly ILogger<FileDataService> _logger;
        private readonly IFileReaderService _fileReaderService;

        public FileDataService(ILogger<FileDataService> logger, IFileReaderService fileReaderService)
        {
            _logger = logger;
            _fileReaderService = fileReaderService;
        }

        public List<Price> GetPrice(string path)
        {

            List<Price> priceList = new List<Price>();
            try
            {
                _logger.LogInformation("GetPrice method start");

                var pricedata = _fileReaderService.ReadTextFile(path);

                foreach (var prices in pricedata)
                {
                    var rate = prices.ToString().Split('=');

                    var price = new Price
                    {
                        FoodType = rate[0],
                        Rate = decimal.Parse(rate[1], CultureInfo.InvariantCulture)
                    };

                    priceList.Add(price);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Price text file is not in correct format");
                _logger.LogError(ex.Message);
                Console.WriteLine("Price text file is not in correct format");
            }

            _logger.LogInformation("GetPrice method exit");

            return priceList;
        }

        public List<Animals> GetAnimals(string path)
        {
            List<Animals> animalsList = new List<Animals>();

            try
            {
                var animalsData = _fileReaderService.ReadCSVFile<Animals>(path);

                foreach (Animals record in animalsData)
                {
                    record.FoodPercentage = record.FoodPercentage.Replace("%", string.Empty);
                    animalsList.Add(record);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Animal CSV file is not in correct format");
                _logger.LogError(ex.Message);
                Console.WriteLine("Animal CSV file is not in correct format");
            }

            return animalsList;
        }

        public List<ZooContent> GetZooContent(string path)
        {
            List<ZooContent> zooContent = new List<ZooContent>();

            try
            {
                var zooData = _fileReaderService.ReadXMLFile(path);

                if (zooData == null)
                    return null;

                foreach (var animal in zooData)
                {
                    foreach (var item in animal.Elements())
                    {
                        var zoo = new ZooContent
                        {
                            Type = item.Name.LocalName,
                            Name = item.FirstAttribute.Value,
                            Weight = decimal.Parse(item.LastAttribute.Value, CultureInfo.InvariantCulture)
                        };
                        zooContent.Add(zoo);
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Zoo content file is not in correct format");
                _logger.LogError(ex.Message);
                Console.WriteLine("Zoo content file is not in correct format");
            }

            return zooContent;
        }
    }
}
