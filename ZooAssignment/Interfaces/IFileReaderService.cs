using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using ZooAssignment.Model;

namespace ZooAssignment.Interfaces
{
    public interface IFileReaderService
    {
        public List<string> ReadTextFile(string path);

        public string ReadCSVFile(string path);

        public IEnumerable<XElement> ReadXMLFile(string path);

    }
}
