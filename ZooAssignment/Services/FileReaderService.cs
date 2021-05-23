
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

        //public DataTable ReadCSVFile(string path)
        //{
        //    //try
        //    //{
        //    //    CsvParserOptions csvParserOptions = new CsvParserOptions(false, ';');
        //    //    CsvUserDetailsMapping csvMapper = new CsvUserDetailsMapping();
        //    //    CsvParser<classmodel> csvParser = new CsvParser<classmodel>(csvParserOptions, mapper);
        //    //    var result = csvParser.ReadFromFile(path, Encoding.ASCII).ToList();
        //    //    foreach (var details in result)
        //    //    {
        //    //        details.Result.FoodPercentage = details.Result.FoodPercentage.Replace("%", string.Empty);
        //    //        animals.Add(details.Result);
        //    //    }
        //    //}
        //    //catch (Exception)
        //    //{

        //    //    throw;
        //    //}


        //    //return animals;

        //    var csvTable = new DataTable();
        //    using (var csvReader = new CsvReader(new StreamReader(System.IO.File.OpenRead(@"D:\CSVFolder\CSVFile.csv")), true))
        //    {
        //        csvTable.Load(csvReader);
        //    }

        //    //var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        //    //{
        //    //    HasHeaderRecord = false,
        //    //};

        //    //var reader = new StreamReader(path);
        //    //var csv = new CsvReader(reader, config);


        //    //return csv;

        //}

        public string ReadCSVFile(string path)
        {
            DataTable csvData = new DataTable();
            string jsonString = string.Empty;
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(path))
                {
                    csvReader.SetDelimiters(new string[] { ";" });
                    csvReader.HasFieldsEnclosedInQuotes = false;

                    string[] colFields;
                    bool tableCreated = false;
                    while (tableCreated == false)
                    {
                        colFields = csvReader.ReadFields();
                        //foreach (string column in colFields)
                        //{
                        //    DataColumn datecolumn = new DataColumn(column);
                        //    datecolumn.AllowDBNull = true;
                        //    csvData.Columns.Add(datecolumn);
                        //}

                        for (int i = 0; i < colFields.Length; i++)
                        {
                            DataColumn datecolumn = new DataColumn("col" + (i + 1));
                            datecolumn.AllowDBNull = true;
                            csvData.Columns.Add(datecolumn);
                        }

                        tableCreated = true;
                    }
                    while (!csvReader.EndOfData)
                    {
                        csvData.Rows.Add(csvReader.ReadFields());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Error:Parsing CSV";
            }
            //if everything goes well, serialize csv to json 
            jsonString = JsonConvert.SerializeObject(csvData);

            return jsonString;
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
