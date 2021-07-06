using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoTracker.Modules
{
    [Serializable]
    public class Position
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
        public decimal CostBasis { get { return Math.Round((decimal)Amount * AverageCost, 2); } }

        public decimal CurrentValue { get { return Math.Round((decimal)Amount * LastPrice, 2); } }

        //Net Profit
        public decimal NetProfit { get; set; }

        public decimal TotalFees { get; set; }

        public Position()
        {
            Amount = 0;
            AverageCost = 0.00m;
            TotalFees = 0.00m;
        }

        public void CalculateNetProfit()
        {
            NetProfit = Math.Round(((decimal)Amount * LastPrice) - CostBasis, 2);
        }

        public override string ToString()
        {
            return $"{Token} - Amount: {Amount}, Average Cost: {AverageCost}, Last Price: {LastPrice}, Cost Basis: {CostBasis}, Net Profit: {NetProfit}";
        }
    }
}
