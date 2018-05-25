using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TradeClient.Helpers;
using TradeClient.Models;
using TradeClient.Views;

namespace TradeClient.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        /* Trading */
        public ObservableCollection<Share> AvailableShares { get; set; }
        public ObservableCollection<Share> MyShares { get; set; }

        public ObservableCollection<User> Users { get; set; }


        public Share SelectedAvailableShare { get; set; }
        public Share SelectedMyShare { get; set; }
        public User CurrentUser { get; set; }

        public ICommand BuySharesCommand { get; set; }
        public ICommand MarkSharesForSaleCommand { get; set; }

        public ICommand RefreshCommand { get; set; }

        /* Admin */

        public ICommand CreateSharesCommand { get; set; }
        public ICommand CreateUserCommand { get; set; }
        public ICommand GenerateUserIdCommand { get; set; }
        public Guid CreateUserGuid { get; set; } = Guid.NewGuid();



        public MainWindowViewModel()
        {
            //StockId, SharePrice

            // Get shares by user ID
            AvailableShares = new ObservableCollection<Share>(){ new Share(){Amount = 1337, StockId = "ASDF", Price = 15.32m}};
            MyShares = new ObservableCollection<Share>(){ new Share() { Amount = 1337, StockId = "ASDF", Price = 20.5m }};
            Users = new ObservableCollection<User>();

            BuySharesCommand = new RelayCommand(OnBuyShares, () => SelectedAvailableShare != null);
            MarkSharesForSaleCommand = new RelayCommand(OnMarkSharesForSale, () => SelectedMyShare != null);

            CreateUserCommand = new RelayCommand(OnCreateUser);
            CreateSharesCommand = new RelayCommand(OnCreateShares);
            RefreshCommand = new RelayCommand(OnRefresh);
            GenerateUserIdCommand = new RelayCommand(OnGenerateUserId);
        }

        private void OnCreateUser()
        {

            //var user = new User(){UserId = CreateUserGuid};

            var exists = Users.Any(user => user.UserId == CreateUserGuid);

            if (!exists)
            {
                //Create user with CreateUserGuid API call
                Users.Add(new User(){UserId = CreateUserGuid});
                Notify("Users");
            }
        }

        private void OnGenerateUserId()
        {
            CreateUserGuid = Guid.NewGuid();
            Notify("CreateUserGuid");
        }

        private void OnRefresh()
        {
            //Get shares and stuff for current user
            //Notify collections changed
        }

        private void OnCreateShares()
        {
            throw new NotImplementedException();
        }


        private void OnMarkSharesForSale()
        {
            var dlg = new PickAmountDialog(SelectedMyShare) {Owner = App.Current.MainWindow};
            if (dlg.ShowDialog() == true)
            {
                var amountToMark = Convert.ToInt32(dlg.AmountTbx.Text);
                //Mark for sale
                //Notify collection?
            }
        }


        private void OnBuyShares()
        {
            var dlg = new PickAmountDialog(SelectedAvailableShare) { Owner = App.Current.MainWindow };
            if (dlg.ShowDialog() == true)
            {
                var amountToMark = Convert.ToInt32(dlg.AmountTbx.Text);
                //Request buy
                //Notify collection?
            }
        }

    }
}