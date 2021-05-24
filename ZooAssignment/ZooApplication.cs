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
                Console.WriteLine("Total cost per day to feed the animals: {0:0.00}", totalCost);
                Console.WriteLine("Thank you for using Zoo expenses calculation system, press any 1 to continue again or press any other key to exit from application");
                _log.LogInformation("Total cost calculation done");

                ConsoleKeyInfo action = Console.ReadKey();
                if (action.KeyChar == '1')
                {
                    Console.WriteLine();
                    Run();
                }

            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                Console.WriteLine("Something went wrong with calculation, please try again");
            }

        }
    }
}
