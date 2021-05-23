using System;
using System.Collections.Generic;
using System.Text;
using ZooAssignment.Model;

namespace ZooAssignment.Interfaces
{
    public interface IFileDataService
    {
        public List<Price> GetPrice();

        public List<Animals> GetAnimals();

        public List<ZooContent> GetZooContent();
    }
}
