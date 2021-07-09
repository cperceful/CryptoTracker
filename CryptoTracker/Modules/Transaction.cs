using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoTracker.Modules
{
    public class Transaction
    {

        //Token
        public string Token { get; set; }
        //Date
        public string Date { get; set; }

        public double Amount { get; set; }

        //transaction type
        public TransactionType transType { get; set; }

        //average cost
        public decimal AverageCost { get; set; }

        //fee
        public double Fee { get; set; }

        public enum TransactionType
        {
            BUY,
            SELL
        }

        public override string ToString()
        {
            return $"Token: {Token}, Date: {Date}, Amount: {Amount}, Transaction Type: {transType}, Average Cost: {AverageCost}, Fee: {Fee}";
        }

    }
}
