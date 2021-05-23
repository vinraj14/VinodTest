using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using ZooAssignment.Model;

namespace ZooAssignment.Interfaces
{
    public interface IFileReaderService
    {
        public List<string> ReadTextFile(string path);       

        public List<T> ReadCSVFile<T>(string path) where T : new();

        public IEnumerable<XElement> ReadXMLFile(string path);

    }
}
