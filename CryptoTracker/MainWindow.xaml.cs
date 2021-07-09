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
            SetUpTransactionType();
            ProcessData();
        }

        private async void ProcessData()
        {
            await DataHelper.ProcessData();

            dgPositions.ItemsSource = DataHelper.Positions;
            dgTransactions.ItemsSource = DataHelper.Transactions;
        }


        private void GetPrice()
        {
            textBlockCurrentPrice.Text = "Fetching Price";

            //textBlockCurrentPrice.Text = DataHelper.GetPrices();

        }

        bool doClose = false;
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!doClose)
            {
                e.Cancel = true;
                Save();
            }
        }

        private void SetUpTransactionType()
        {
            cmbTransactionType.Items.Add(Transaction.TransactionType.BUY.ToString());
            cmbTransactionType.Items.Add(Transaction.TransactionType.SELL.ToString());
            cmbTransactionType.SelectedIndex = 0;
        }

        private void btnAddTransaction_Click(object sender, RoutedEventArgs e)
        {
            Transaction transaction = new Transaction();

            bool isInputValid = true;
            if (string.IsNullOrWhiteSpace(txtToken.Text))
            {
                isInputValid = false;
            }
            else
            {
                transaction.Token = txtToken.Text;
            }

            if (!dtDate.SelectedDate.HasValue)
            {
                isInputValid = false;
            }
            else
            {                
                transaction.Date = dtDate.SelectedDate.Value.ToShortDateString();
            }

            if (string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                isInputValid = false;
            }
            else
            {
                if (double.TryParse(txtAmount.Text, out double amount))
                {
                    transaction.Amount = amount;
                }
                else
                {
                    isInputValid = false;
                }
            }

            if (string.IsNullOrWhiteSpace(cmbTransactionType.Text))
            {
                isInputValid = false;
            }
            else
            {
                if (cmbTransactionType.Text == "BUY")
                {
                    transaction.transType = Transaction.TransactionType.BUY;
                }
                else
                {
                    transaction.transType = Transaction.TransactionType.SELL;
                }
            }

            if (string.IsNullOrWhiteSpace(txtAverageCost.Text))
            {
                isInputValid = false;
            }
            else
            {
                if (decimal.TryParse(txtAverageCost.Text, out decimal averageCost))
                {
                    transaction.AverageCost = averageCost;
                }
                else
                {
                    isInputValid = false;
                }
            }

            if (string.IsNullOrWhiteSpace(txtFee.Text))
            {
                transaction.Fee = 0.00;
            }
            else
            {
                if (double.TryParse(txtFee.Text, out double fee))
                {
                    transaction.Fee = fee;
                }
                else
                {
                    isInputValid = false;
                }
            }

            if (isInputValid)
            {
                DataHelper.AddTransaction(transaction);
                DataHelper.SaveFile();
                ProcessData();
                tiPositions.IsSelected = true;
                txtToken.Text = "";
                dtDate.SelectedDate = DateTime.Today;
                txtAmount.Text = "";
                txtFee.Text = "";
            }
        }

        private async void Save()
        {
            await DataHelper.SaveFile();
            doClose = true;
            Close();
        }
    }
}
