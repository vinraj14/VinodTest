
using System;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ZooAssignment.Interfaces;
using CsvHelper;
using System.Globalization;
using ZooAssignment.Model;
using CsvHelper.Configuration;

namespace ZooAssignment.Services
{
    public class FileReaderService : IFileReaderService
    {
        private readonly ILogger<FileReaderService> _logger;
     
        public FileReaderService(ILogger<FileReaderService> logger)
        {
            _logger = logger;           
        }

        public List<string> ReadTextFile(string path)
        {
            List<string> txtData = new List<string>();
            try
            {
                _logger.LogInformation("ReadTextFile method start");
                FileStream fileStream = new FileStream(path, FileMode.Open);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        txtData.Add(line);
                    }
                }

                if (txtData.Count == 0) {
                    Console.WriteLine("Price text file has no data, please check your file");
                }
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError("File cannot be found");
                _logger.LogError(ex.Message);
                Console.WriteLine("File cannot be found");
            }
            catch (FileLoadException ex)
            {
                _logger.LogError("File cannot be loaded");
                _logger.LogError(ex.Message);
                Console.WriteLine("File cannot be loaded");
            }
            catch (Exception e)
            {
                _logger.LogError("Error while returning data, please check your text file");
                _logger.LogError(e.Message);
                Console.WriteLine("Error while returning data, please check your text file");
            }

            _logger.LogInformation("ReadTextFile method exit");



            return txtData;
        }

        public List<T> ReadCSVFile<T>(string path) where T : new()
        {
            List<T> records = new List<T>();

            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                    MissingFieldFound = null,
                    Delimiter = ";"
                };

                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, config))
                {
                    records.AddRange(csv.GetRecords<T>());
                }

                if (records.Count == 0)
                {
                    Console.WriteLine("Animal CSV file has no data, please check your file");
                }

            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError("File cannot be found");
                _logger.LogError(ex.Message);
                Console.WriteLine("File cannot be found");
            }
            catch (FileLoadException ex)
            {
                _logger.LogError("File cannot be loaded");
                _logger.LogError(ex.Message);
                Console.WriteLine("File cannot be loaded");
            }
            catch (Exception e)
            {
                _logger.LogError("Error while returning data, please check your csv file");
                _logger.LogError(e.Message);
                Console.WriteLine("Error while returning data, please check your csv file");
            }

            return records;

        }

        public IEnumerable<XElement> ReadXMLFile(string path)
        {
            IEnumerable<XElement> xElements = null;
          
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                var xdoc = XDocument.Load(path);
                xElements = xdoc.Root.Elements();

            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError("File cannot be found");
                _logger.LogError(ex.Message);
                Console.WriteLine("File cannot be found");
            }
            catch (FileLoadException ex)
            {
                _logger.LogError("File cannot be loaded");
                _logger.LogError(ex.Message);
                Console.WriteLine("File cannot be loaded");
            }
            catch (Exception e)
            {
                _logger.LogError("xml file has not data, please check your file");
                _logger.LogError(e.Message);
                Console.WriteLine("xml file has not data, please check your file");
            }

            return xElements;
        }

    }
}
