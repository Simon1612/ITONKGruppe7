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

        public Share SelectedAvailableShare { get; set; }
        public Share SelectedMyShare { get; set; }

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
                }

                else if (!(_currentUser.UserId == value.UserId))
                {
                    _currentUser = value;
                    OnRefresh();
                }
            }
        }
        public int CurrentUserIndex { get; set; }

        public ICommand BuySharesCommand { get; set; }
        public ICommand MarkSharesForSaleCommand { get; set; }

        //public ICommand RefreshCommand { get; set; }

        /* Admin */
        public ICommand CreateSharesCommand { get; set; }
        public ICommand CreateUserCommand { get; set; }
        public ICommand GenerateUserIdCommand { get; set; }
        public Guid CreateUserGuid { get; set; } = Guid.NewGuid();


        /* Common */
        public ObservableCollection<User> Users { get; set; }


        public MainWindowViewModel()
        {
            AvailableShares = new ObservableCollection<Share>() {new Share() {Amount = 1337, StockId = "ASDF", Price = 15.32m}};
            MyShares = new ObservableCollection<Share>() {new Share() {Amount = 1337, StockId = "ASDF", Price = 20.5m}};
            Users = new ObservableCollection<User>();

            BuySharesCommand = new RelayCommand(OnBuyShares, () => SelectedAvailableShare != null);
            MarkSharesForSaleCommand = new RelayCommand(OnMarkSharesForSale, () => SelectedMyShare != null);

            CreateUserCommand = new RelayCommand(OnCreateUser);
            CreateSharesCommand = new RelayCommand(OnCreateShares);
            //RefreshCommand = new RelayCommand(OnRefresh);
            GenerateUserIdCommand = new RelayCommand(OnGenerateUserId);
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

        private void OnRefresh()
        {
            var asdf = 5;
            //Get shares and stuff for current user
            //Notify collections changed
        }


        #endregion


        #region Admin

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
                    //Create Share with no owner?
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