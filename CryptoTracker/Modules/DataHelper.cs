using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CryptoTracker.Modules;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace CryptoTracker
{
    public static class DataHelper
    {
        private const string BASE_URL = "https://api.nomics.com/v1";
        

        private static readonly HttpClient client = new HttpClient();

        private static string path = AppDomain.CurrentDomain.BaseDirectory;
        private static string file = "transactions.json";
        private static string fullPath = $"{path}\\{file}";

        public static List<Transaction> Transactions { get; private set; }
        public static Dictionary<string, List<Transaction>> TransactionDictionary { get; private set; }
        public static List<Position> Positions { get; private set; }
        public static string Ids { get; private set; }

        public static void ProcessData()
        {
            LoadFile();
            CalculatePositions();
            GetAllTransactions();
            GetIds();
            GetPrices();
            foreach (Position position in Positions)
            {
                position.CalculateNetProfit();
            }
        }

        public static void AddTransaction(Transaction transaction)
        {
            string token = transaction.Token;

            if (TransactionDictionary == null)
            {
                TransactionDictionary = new Dictionary<string, List<Transaction>>();
            }

            if (TransactionDictionary.ContainsKey(token))
            {
                TransactionDictionary[token].Add(transaction);
            }
            else
            {
                List<Transaction> newTokenTransactions = new List<Transaction>();
                newTokenTransactions.Add(transaction);
                TransactionDictionary.Add(token, newTokenTransactions);
            }
        }

        public static void GetPrices()
        {
            string requestURL = $"{BASE_URL}/currencies/ticker?key={Credentials.API_KEY}&ids={Ids}&interval=1d";
            Task<string> response = client.GetStringAsync(requestURL);
            List<PriceResult> priceResults = JsonSerializer.Deserialize<List<PriceResult>>(response.Result);
            foreach (PriceResult price in priceResults)
            {
                foreach (Position position in Positions)
                {
                    if (position.Token.Equals(price.Id))
                    {
                        bool isSuccessful = decimal.TryParse(price.Price, out decimal lastPrice);
                        if (isSuccessful)
                            position.LastPrice = Math.Round(lastPrice, 2);
                    }
                }
            }
        }

        public static void SaveFile()
        {
            string json = JsonSerializer.Serialize(TransactionDictionary);
            File.WriteAllText(fullPath, json);
            
        }

        private static void LoadFile()
        {
            string fileContents = File.ReadAllText(fullPath);
            if (!string.IsNullOrWhiteSpace(fileContents))
            {
                TransactionDictionary = JsonSerializer.Deserialize<Dictionary<string, List<Transaction>>>(fileContents);
            }
        }

        private static void CalculatePositions()
        {
            Positions = new List<Position>();
            foreach (KeyValuePair<string, List<Transaction>> transactionList in TransactionDictionary)
            {
                Position position = new Position();
                position.Token = transactionList.Key;
                decimal totalCost = 0.00m;
                foreach (Transaction transaction in transactionList.Value)
                {
                    position.Amount += transaction.Amount;
                    totalCost += transaction.AverageCost;
                    position.TotalFees += transaction.Fee;
                }
                position.AverageCost = Math.Round(totalCost / transactionList.Value.Count, 2);
                Positions.Add(position);
            }
        }

        private static void GetAllTransactions()
        {
            Transactions = new List<Transaction>();
            foreach (KeyValuePair<string, List<Transaction>> keyValuePair in TransactionDictionary)
            {
                foreach (Transaction transaction in keyValuePair.Value)
                {
                    Transactions.Add(transaction);
                }
            }
        }

        private static void GetIds()
        {
            List<string> idList = new List<string>();
            foreach (Position position in Positions)
            {
                idList.Add(position.Token);
            }
            string[] idArray = idList.ToArray();
            Ids = string.Join(",", idArray);
        }

    }


}
