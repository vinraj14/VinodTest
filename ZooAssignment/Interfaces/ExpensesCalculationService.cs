using System;
using System.Collections.Generic;
using System.Text;
using ZooAssignment.Model;

namespace ZooAssignment.Interfaces
{  
    public interface IExpensesCalculationService
    {
        public List<Price> GetPrice();
        public List<Animals> GetAnimals();
        public List<ZooContent> GetZooContents();
        public decimal GetTotalCost();
    }
}
