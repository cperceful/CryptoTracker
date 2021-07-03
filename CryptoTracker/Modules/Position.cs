using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoTracker.Modules
{
    [Serializable]
    class Position
    {
        //Token
        public string Token { get; set; }

        //Amount
        public double Amount { get; set; }

        //Average Cost
        public decimal AverageCost { get; set; }
        
        //Last pulled price
        public decimal LastPrice { get; set; }

        //Cost Basis
        public decimal CostBasis { get { return (decimal)Amount * AverageCost; } }

        //Net Profit
        public decimal NetProfit { get; set; }

        public void CalculateNetProfit()
        {
            NetProfit = ((decimal)Amount * LastPrice) - CostBasis;
        }
    }
}
