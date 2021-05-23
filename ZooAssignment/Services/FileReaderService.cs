
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
            catch (Exception e)
            {
                _log.LogError("The file could not be read:");
                _log.LogError(e.Message);
            }

            return txtData;
        }
          
        public List<T> ReadCSVFile<T>(string path) where T : new()
        {
            List<T> records = new List<T>();

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
            catch (Exception)
            {
                throw;
            }

            return xElements;
        }

    }
}
