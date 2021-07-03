using CryptoTracker.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CryptoTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Position position1 = new Position
            {
                Token = "ADA",
                Amount = 1000,
                LastPrice = 1.00m,
                AverageCost = 0.99m
            };
            position1.CalculateNetProfit();

            Position position2 = new Position
            {
                Token = "ONE",
                Amount = 10000,
                LastPrice = 0.09m,
                AverageCost = 0.10m
            };
            position2.CalculateNetProfit();

            Holdings holdings = new Holdings();
            holdings.Positions.Add(position1);
            holdings.Positions.Add(position2);

            listViewHoldings.DataContext = holdings.Positions;
        }

        private async void btnGetPrice_Click(object sender, RoutedEventArgs e)
        {
            //textBlockCurrentPrice.Text = "Fetching Price";

            //textBlockCurrentPrice.Text = await APIHelper.GetPrice();

        }
    }
}
