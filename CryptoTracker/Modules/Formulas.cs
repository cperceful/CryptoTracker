using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoTracker.Modules
{
    public static class Formulas
    {
        public static decimal CalculateNetProfit(double amount, decimal averageCost, decimal lastPrice)
        {
            decimal profit;
            profit = ((decimal)amount * lastPrice) - ((decimal)amount * averageCost);
            return profit;
        }
    }
}
