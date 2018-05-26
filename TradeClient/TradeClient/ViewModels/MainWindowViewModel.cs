using System;
using System.Collections.ObjectModel;
using System.Dynamic;
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
        public ObservableCollection<Share> MyMarkedShares { get; set; }

        public Share SelectedAvailableShare { get; set; }
        public Share SelectedMyShare { get; set; }
        public Share SelectedMyMarkedShare { get; set; }

        private User _currentUser;
        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                if (_currentUser == null)
                {
                    _currentUser = value;
                    OnRefresh();
                    Notify("CurrentUser");
                }

                else if (!(_currentUser.UserId == value.UserId))
                {
                    _currentUser = value;
                    OnRefresh();
                    Notify("CurrentUser");

                }
            }
        }
        public int CurrentUserIndex { get; set; }

        public ICommand BuySharesCommand { get; set; }
        public ICommand MarkSharesForSaleCommand { get; set; }
        public ICommand UnmarkSharesCommand { get; set; }


        public ICommand RefreshCommand { get; set; }

        /* Admin */
        public ICommand SwitchUserCommand { get; set; }
        public ICommand CreateSharesCommand { get; set; }
        public ICommand CreateUserCommand { get; set; }
        public ICommand GenerateUserIdCommand { get; set; }
        public Guid CreateUserGuid { get; set; } = Guid.NewGuid();
        public User SelectedUser { get; set; }


        /* Common */
        public ObservableCollection<User> Users { get; set; }


        public MainWindowViewModel()
        {
            //TODO: Init collections empty and get list of users, available shares, all my shares, my marked shares


            var availableShare = new Share() {Amount = 30, StockId = "ABC", Price = 15.32m};
            AvailableShares = new ObservableCollection<Share>() {availableShare};

            var markedShare = new Share() {Amount = 10, StockId = "GHI", Price = 2.352m};
            MyMarkedShares = new ObservableCollection<Share>() {markedShare};

            var myShare = new Share() {Amount = 20, StockId = "DEF", Price = 20.5m};
            MyShares = new ObservableCollection<Share>() {myShare, markedShare};

            var user = new User() {UserId = Guid.NewGuid()};
            Users = new ObservableCollection<User>(){user};
            CurrentUser = user;


            /* Trading */
            BuySharesCommand = new RelayCommand(OnBuyShares, () => SelectedAvailableShare != null);
            MarkSharesForSaleCommand = new RelayCommand(OnMarkSharesForSale, () => SelectedMyShare != null);
            UnmarkSharesCommand = new RelayCommand(OnUnmarkShares, () => SelectedMyMarkedShare != null);

            /* Admin */
            CreateUserCommand = new RelayCommand(OnCreateUser);
            CreateSharesCommand = new RelayCommand(OnCreateShares);
            RefreshCommand = new RelayCommand(OnRefresh);
            GenerateUserIdCommand = new RelayCommand(OnGenerateUserId);
            SwitchUserCommand = new RelayCommand(OnSwitchUser, () => SelectedUser != null && SelectedUser.UserId != CurrentUser.UserId);
        }


        #region Trading

        private void OnMarkSharesForSale()
        {
            var dlg = new PickAmountDialog(SelectedMyShare) {Owner = App.Current.MainWindow};
            if (dlg.ShowDialog() == true)
            {
                var amountToMark = Convert.ToInt32(dlg.AmountTbx.Text);
                //Mark for sale
                //Add and Notify collection or refresh?
            }
        }

        private void OnUnmarkShares()
        {
            //API call to unmark with info from SelectedMyMarkedShare
            MyMarkedShares.Remove(SelectedMyMarkedShare);
        }

        private void OnBuyShares()
        {
            var dlg = new PickAmountDialog(SelectedAvailableShare) {Owner = App.Current.MainWindow};
            if (dlg.ShowDialog() == true)
            {
                var amountToMark = Convert.ToInt32(dlg.AmountTbx.Text);
                //Request buy
                //Add and Notify collection or refresh?        
            }

        }

        //Gets called every time CurrentUser property is set
        private void OnRefresh()
        {
            //Get shares and stuff for current user
            //Notify collections changed
        }


        #endregion


        #region Admin

        private void OnSwitchUser()
        {
            CurrentUser = SelectedUser;
        }

        private void OnCreateUser()
        {

            //var user = new User(){UserId = CreateUserGuid};

            var userExists = Users.Any(user => user.UserId == CreateUserGuid);

            if (!userExists)
            {
                //Create user with CreateUserGuid API call
                Users.Add(new User() {UserId = CreateUserGuid});
                Notify("Users");
            }
        }

        private void OnCreateShares()
        {
            var dlg = new CreateShareDialog(Users) {Owner = App.Current.MainWindow};
            if (dlg.ShowDialog() == true)
            {
                var newShare = new Share()
                {
                    StockId = dlg.StockIdTbx.Text.ToUpper(),
                    Price = Convert.ToDecimal(dlg.PriceTbx.Text),
                    Amount = Convert.ToInt32(dlg.AmountTbx.Text)
                };

                var selectedOwner = dlg.SelectUserCbx.SelectionBoxItem;

                //If no owner selected
                if (String.IsNullOrWhiteSpace(selectedOwner.ToString()))
                {
                    //Create Share with no owner? if no owner share should automatically be marked for sale and immediately be added to availableshares list

                }
                else
                {
                    var owner = selectedOwner as User;
                    //Create share with owner


                }
            }
        }

        private void OnGenerateUserId()
        {
            CreateUserGuid = Guid.NewGuid();
            Notify("CreateUserGuid");
        }


        #endregion

    }
}