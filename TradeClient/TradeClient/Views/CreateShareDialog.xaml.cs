using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// Interaction logic for CreateShareDialog.xaml
    /// </summary>
    public partial class CreateShareDialog : Window
    {
        public ICommand OkCommand { get; set; }
        public ObservableCollection<User> Users { get; set; }
        public User SelectedUser { get; set; } = null;

        public int PickedAmount { get; set; } = 0;

        public CreateShareDialog(ObservableCollection<User> users)
        {
            InitializeComponent();
            Users = users;

            OkCommand = new RelayCommand(OnOkClicked, CanExecuteOk);
            DataContext = this;
        }

        private bool CanExecuteOk()
        {
            // Regex 1: Success on whole numbers > 0 https://stackoverflow.com/questions/7036324/what-is-the-regex-for-any-positive-integer-excluding-0
            //Regex 2: Success on any positive number https://stackoverflow.com/questions/4466002/regex-that-matches-positive-numbers
            return Regex.Match(AmountTbx.Text, @"^[1-9]\d*$").Success 
                   && Regex.Match(PriceTbx.Text, @"^[+]?\d+([.]\d+)?$").Success 
                   && Convert.ToDecimal(PriceTbx.Text) != 0 
                   && !String.IsNullOrWhiteSpace(StockIdTbx.Text)
                   && !String.IsNullOrWhiteSpace(SelectUserCbx.SelectionBoxItem.ToString());
        }

        private void OnOkClicked()
        {
            App.Current.MainWindow.OwnedWindows[0].DialogResult = true;
        }
    }
}

