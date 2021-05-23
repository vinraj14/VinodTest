
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
        private readonly ILogger<FileReaderService> _log;
        private readonly IConfiguration _config;

        public FileReaderService(ILogger<FileReaderService> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        public List<string> ReadTextFile(string path)
        {
            List<string> txtData = new List<string>();
            try
            {
                FileStream fileStream = new FileStream(path, FileMode.Open);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        txtData.Add(line);
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                _log.LogError("File cannot be found");
                _log.LogError(ex.Message);
                Console.WriteLine("File cannot be found");
            }
            catch (FileLoadException ex)
            {
                _log.LogError("File cannot be loaded");
                _log.LogError(ex.Message);
                Console.WriteLine("File cannot be loaded");
            }
            catch (Exception e)
            {
                _log.LogError("Error while returning data, please check your text file");
                _log.LogError(e.Message);
                Console.WriteLine("Error while returning data, please check your text file");
            }

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
            }
            catch (FileNotFoundException ex)
            {
                _log.LogError("File cannot be found");
                _log.LogError(ex.Message);
                Console.WriteLine("File cannot be found");
            }
            catch (FileLoadException ex)
            {
                _log.LogError("File cannot be loaded");
                _log.LogError(ex.Message);
                Console.WriteLine("File cannot be loaded");
            }
            catch (Exception e)
            {
                _log.LogError("Error while returning data, please check your csv file");
                _log.LogError(e.Message);
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
                _log.LogError("File cannot be found");
                _log.LogError(ex.Message);
                Console.WriteLine("File cannot be found");
            }
            catch (FileLoadException ex)
            {
                _log.LogError("File cannot be loaded");
                _log.LogError(ex.Message);
                Console.WriteLine("File cannot be loaded");
            }
            catch (Exception e)
            {
                _log.LogError("Error while returning data, please check your xml file");
                _log.LogError(e.Message);
                 Console.WriteLine("Error while returning data, please check your xml file");
            }

            return xElements;
        }

    }
}
