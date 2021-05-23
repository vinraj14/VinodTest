using System;
using System.Collections.Generic;
using System.Text;
using ZooAssignment.Model;

namespace ZooAssignment.Interfaces
{
    public interface IFileDataService
    {
        public List<Price> GetPrice(string path);

        public List<Animals> GetAnimals(string path);

        public List<ZooContent> GetZooContent(string path);
    }
}
