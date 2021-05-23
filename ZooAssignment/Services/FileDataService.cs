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
                //FileStream fileStream = new FileStream(path, FileMode.Open);
                //using (StreamReader reader = new StreamReader(fileStream))
                //{
                //    string line = "";
                //    while ((line = reader.ReadLine()) != null)
                //    {
                //        var rate = line.Split('=');
                //        var price = new Price
                //        {
                //            FoodType = rate[0],
                //            Rate = decimal.Parse(rate[1], CultureInfo.InvariantCulture)
                //        };
                //        prices.Add(price);
                //    }
                //}

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
                // Something went wrong.
                _log.LogError("The file could not be read:");
                //print error message
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
                var animlasData = _fileReaderService.ReadCSVFile(path);

                //CsvParserOptions csvParserOptions = new CsvParserOptions(false, ';');
                //CsvUserDetailsMapping csvMapper = new CsvUserDetailsMapping();
                //CsvParser<Animals> csvParser = new CsvParser<Animals>(csvParserOptions, csvMapper);
                //var result = csvParser.ReadFromFile(path, Encoding.ASCII).ToList();
                //foreach (DataRow row in animlasData.Rows)
                //{


                //    //details.Result.FoodPercentage = details.Result.FoodPercentage.Replace("%", string.Empty);
                //    //animals.Add(details.Result);

                //    Console.WriteLine(row);
                //}

                //var records = animlasData.GetRecords<Animals>();



                List<Animals> animals = (List<Animals>)JsonConvert.DeserializeObject(animlasData, (typeof(List<Animals>)));

                foreach (Animals record in animals)
                {
                    record.FoodPercentage = record.FoodPercentage.Replace("%", string.Empty);
                    animals.Add(record);
                    Console.WriteLine(record);
                }

            }
            catch (Exception e)
            {

                // Something went wrong.
                _log.LogError("The file could not be read:");
                //print error message
                _log.LogError(e.Message);
            }

            return animalsList;
        }

        public List<ZooContent> GetZooContent()
        {
            List<ZooContent> zooContent = new List<ZooContent>();
            //XmlReaderSettings settings = new XmlReaderSettings();
            //settings.IgnoreWhitespace = true;


            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _config.GetValue<string>("ZooContentPath"));

            var zooData = _fileReaderService.ReadXMLFile(path);

            //var xdoc = XDocument.Load(path);
            //IEnumerable<XElement> animals = xdoc.Root.Elements();

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
