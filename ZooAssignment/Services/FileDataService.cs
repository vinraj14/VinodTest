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
        private readonly ILogger<FileDataService> _log;
        private readonly IConfiguration _config;
        private readonly IFileReaderService _fileReaderService;

        public FileDataService(ILogger<FileDataService> log, IConfiguration config, IFileReaderService fileReaderService)
        {
            _log = log;
            _config = config;
            _fileReaderService = fileReaderService;
        }

        public List<Price> GetPrice()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _config.GetValue<string>("PriceTextPath"));
            List<Price> priceList = new List<Price>();
            try
            { 
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
            catch (Exception e)
            {               
                _log.LogError("The file could not be read:");             
                _log.LogError(e.Message);
            }

            return priceList;
        }

        public List<Animals> GetAnimals()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _config.GetValue<string>("AnimalCSVPath"));
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
            catch (Exception e)
            {               
                _log.LogError("The file could not be read:");
             
                _log.LogError(e.Message);
            }

            return animalsList;
        }

        public List<ZooContent> GetZooContent()
        {
            List<ZooContent> zooContent = new List<ZooContent>();  

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _config.GetValue<string>("ZooContentPath"));

            var zooData = _fileReaderService.ReadXMLFile(path);          

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

            return zooContent;
        }
    }
}
