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

        private static string pantryRequest = $"https://getpantry.cloud/apiv1/pantry/{Credentials.PANTRY_ID}/basket/transactions";

        public static List<Transaction> Transactions { get; private set; }
        public static Dictionary<string, List<Transaction>> TransactionDictionary { get; private set; }
        public static List<Position> Positions { get; private set; }
        public static string Ids { get; private set; }

        public static async Task ProcessData()
        {
            await LoadFile();
            //LoadFileOld();
            CalculatePositions();
            GetAllTransactions();
            GetIds();
            GetPrices();
            GetProfits();
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

        public static async Task SaveFile()
        {
            string json = JsonSerializer.Serialize(TransactionDictionary);
            StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(pantryRequest, stringContent);
            response.EnsureSuccessStatusCode();
        }

        private static async Task LoadFile()
        {

            string response = await client.GetStringAsync(pantryRequest);
            TransactionDictionary = JsonSerializer.Deserialize<Dictionary<string, List<Transaction>>>(response);
        }

        private static void LoadFileOld()
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
                double amountPurchased = 0;
                foreach (Transaction transaction in transactionList.Value)
                {
                    amountPurchased += transaction.Amount;
                    totalCost += transaction.AverageCost * (decimal)transaction.Amount;
                    position.TotalFees += transaction.Fee;
                }
                position.Amount = amountPurchased - position.TotalFees;
                position.AverageCost = Math.Round(totalCost / (decimal)position.Amount, 2);
                Positions.Add(position);
            }
        }

        private static void GetProfits()
        {
            foreach (Position position in Positions)
            {
                position.CalculateNetProfit();
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
