using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TradeClient.Helpers;
using TradeClient.Models;

namespace TradeClient.Views
{
    /// <summary>
    /// Interaction logic for PickAmountDialog.xaml
    /// </summary>
    public partial class PickAmountDialog 
    {
        public ICommand OkCommand { get; set; }
        public Share CurrentShare { get; set; }

        public int PickedAmount { get; set; } = 0;

        public PickAmountDialog(Share share)
        {

            InitializeComponent();
            CurrentShare = share;

            OkCommand = new RelayCommand(OnOkClicked, CanExecuteOk);
            DataContext = this;
        }

        private bool CanExecuteOk()
        {
            return Regex.Match(AmountTbx.Text, @"^[1-9]\d*$").Success && Convert.ToInt32(AmountTbx.Text) <= CurrentShare.Amount;
        }

        private void OnOkClicked()
        {
            App.Current.MainWindow.OwnedWindows[0].DialogResult = true;
        }
    }
}
