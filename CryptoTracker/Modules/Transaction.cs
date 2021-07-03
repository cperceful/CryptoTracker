using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoTracker.Modules
{
    class Transaction
    {
        //Date
        public DateTime Date { get; set; }

        //Token
        public Token TokenType { get; set; }

        //transaction type
        public TransactionType TranType { get; set; }

        //average cost
        public decimal AverageCost { get; set; }

        //fee
        public decimal Fee { get; set; }

        #region enums
        public enum Token
        {
            ADA,
            ONE,
            VET, 
            BTT
        }

        public enum TransactionType
        {
            BUY,
            SELL,
            TRANSFER
        }
        #endregion

    }
}
