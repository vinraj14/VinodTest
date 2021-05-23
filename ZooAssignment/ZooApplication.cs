using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using ZooAssignment.Interfaces;

namespace ZooAssignment
{
    public class ZooApplication
    {
        private readonly ILogger<ZooApplication> _log;
        private readonly IExpensesCalculationService _expensesCalculationService;

        public ZooApplication(ILogger<ZooApplication> log, IExpensesCalculationService expensesCalculationService)
        {
            _log = log;
            _expensesCalculationService = expensesCalculationService;
        }

        public void Run()
        {
            try
            {
                _log.LogInformation("Total cost calculation started");
                var totalCost = _expensesCalculationService.GetTotalCost();
                Console.WriteLine("Total cost per day to feed the animals: {0}", totalCost);
                _log.LogInformation("Total cost calculation done");
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw new Exception("Something went wrong with calculation");
            }

        }
    }
}
 