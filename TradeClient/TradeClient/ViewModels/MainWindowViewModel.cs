using System;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TradeClient.Clients;
using TradeClient.Helpers;
using TradeClient.Models;
using TradeClient.Views;

namespace TradeClient.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        /* Trading */
        public ObservableCollection<AvailableSharesDataModel> AvailableShares { get; set; }
        public ObservableCollection<ShareOwnerDataModel> MyShares { get; set; }
        public ObservableCollection<ShareOwnerDataModel> MyMarkedShares { get; set; }

        public ShareOwnerDataModel SelectedAvailableShare { get; set; }
        public ShareOwnerDataModel SelectedMyShare { get; set; }
        public ShareOwnerDataModel SelectedMyMarkedShare { get; set; }

        private OwnerDataModel _currentUser;

        public OwnerDataModel CurrentUser
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

                else if (!(_currentUser.ShareHolderId == value.ShareHolderId))
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
        public OwnerDataModel SelectedUser { get; set; }


        /* Common */
        public ObservableCollection<OwnerDataModel> Users { get; set; }


        public MainWindowViewModel()
        {

            //var userGuid = Guid.NewGuid();

            //var availableShare = new AvailableSharesDataModel()
            //{
            //    StockOwner = userGuid,
            //    SharesAmount = 1,
            //    StockId = "ABC"
            //};
            //AvailableShares = new ObservableCollection<AvailableSharesDataModel>() { availableShare };

            //var markedShare = new ShareOwnerDataModel()
            //{
            //    ShareOwner = new OwnerDataModel() { ShareHolderId = userGuid },
            //    SharesAmount = 2,
            //    Stock = new StockDataModel() { SharePrice = 2, StockId = "DEF" }
            //};

            //MyMarkedShares = new ObservableCollection<ShareOwnerDataModel>() { markedShare };


            //var myShare = new ShareOwnerDataModel()
            //{
            //    ShareOwner = new OwnerDataModel() { ShareHolderId = userGuid },
            //    SharesAmount = 3,
            //    Stock = new StockDataModel() { SharePrice = 3, StockId = "GHI" }
            //};

            //MyShares = new ObservableCollection<ShareOwnerDataModel>() { myShare, markedShare };

            //var user = new OwnerDataModel() { ShareHolderId = userGuid };
            //Users = new ObservableCollection<OwnerDataModel>() { user };
            //CurrentUser = user;

            AvailableShares = new ObservableCollection<AvailableSharesDataModel>();
            MyMarkedShares = new ObservableCollection<ShareOwnerDataModel>();
            MyShares = new ObservableCollection<ShareOwnerDataModel>();
            Users = new ObservableCollection<OwnerDataModel>();

            /*Available shares*/
            var stockShareProviderClient = new StockShareProviderClient("http://localhost:8748");

            AvailableShares = stockShareProviderClient.ApiStockShareProviderGetAsync().Result;
            Notify("AvailableShares");

            /* Users */
            var shareOwnerControlClient = new ShareOwnerControlClient("http://localhost:8758");
            Users = shareOwnerControlClient.ApiShareOwnerGetAllUsersGetAsync().Result;
            Notify("Users");

            /* If any user already exists in db, set CurrentUser */
            if (Users.Count > 0)
            {
                CurrentUser = Users.First();
                OnRefresh();
            }
            else
            {
                /* Create new user with GUID */
                shareOwnerControlClient.ApiShareOwnerCreateOwnerPostAsync(CreateUserGuid);

                CurrentUser = new OwnerDataModel { ShareHolderId = CreateUserGuid };
            }


            /* Trading */
            BuySharesCommand = new RelayCommand(OnBuyShares, () => SelectedAvailableShare != null);
            MarkSharesForSaleCommand = new RelayCommand(OnMarkSharesForSale, () => SelectedMyShare != null);
            UnmarkSharesCommand = new RelayCommand(OnUnmarkShares, () => SelectedMyMarkedShare != null);

            /* Admin */
            CreateUserCommand = new RelayCommand(OnCreateUser);
            CreateSharesCommand = new RelayCommand(OnCreateShares);
            RefreshCommand = new RelayCommand(OnRefresh);
            GenerateUserIdCommand = new RelayCommand(OnGenerateUserId);
            SwitchUserCommand = new RelayCommand(OnSwitchUser,
                () => SelectedUser != null && SelectedUser.ShareHolderId != CurrentUser.ShareHolderId);
        }
        

        #region Trading

        private void OnMarkSharesForSale()
        {
            var dlg = new PickAmountDialog(SelectedMyShare) {Owner = Application.Current.MainWindow};
            if (dlg.ShowDialog() == true)
            {
                var amountToMark = Convert.ToInt32(dlg.AmountTbx.Text);
                var stockId = dlg.StockIdTbx.Text;

                var stockShareProviderClient = new StockShareProviderClient("http://localhost:8748");
                stockShareProviderClient.ApiStockShareProviderCreateAvailableSharesByStockIdPostAsync(stockId,
                    CurrentUser.ShareHolderId, amountToMark);
                OnRefresh();
            }
        }

        private void OnUnmarkShares()
        {
            //TODO: ?

        }

        private void OnBuyShares()
        {
            var dlg = new PickAmountDialog(SelectedAvailableShare) {Owner = App.Current.MainWindow};
            if (dlg.ShowDialog() == true)
            {
                var amountToMark = Convert.ToInt32(dlg.AmountTbx.Text);
                var stockId = dlg.StockIdTbx.Text;


                var stockShareRequesterClient = new StockShareRequesterClient("http://localhost:8378");
                stockShareRequesterClient.ApiStockShareRequesterInitiateTradeByStockIdBySharesAmountPostAsync(stockId,
                    amountToMark, CurrentUser.ShareHolderId);
                OnRefresh();
            }
        }

        //Gets called every time CurrentUser property is set
        private void OnRefresh()
        {
            /*MyShares*/
            var shareOwnerControlClient = new ShareOwnerControlClient("http://localhost:8758");

            MyShares = shareOwnerControlClient.ApiShareOwnerGetAllSharesForUserByUserIdGetAsync(CurrentUser.ShareHolderId.Value).Result;
            Notify("MyShares");

            /*Available shares*/
            var stockShareProviderClient = new StockShareProviderClient("http://localhost:8748");

            AvailableShares = stockShareProviderClient.ApiStockShareProviderGetAsync().Result;
            Notify("AvailableShares");


            /*Users*/
            Users = shareOwnerControlClient.ApiShareOwnerGetAllUsersGetAsync().Result;
            Notify("Users");
        }

        #endregion


        #region Admin

        private void OnSwitchUser()
        {
            CurrentUser = SelectedUser;
            OnRefresh();
        }

        private void OnCreateUser()
        {
            var userExists = Users.Any(user => user.ShareHolderId == CreateUserGuid);

            if (userExists) return;

            var shareOwnerControlClient = new ShareOwnerControlClient("http://localhost:8758");
            shareOwnerControlClient.ApiShareOwnerCreateOwnerPostAsync(CreateUserGuid);

            OnRefresh();
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

                var owner = dlg.SelectUserCbx.SelectionBoxItem as User;

                var shareOwnerControlClient = new ShareOwnerControlClient("http://localhost:8758");

                shareOwnerControlClient.ApiShareOwnerCreateStockByStockIdPostAsync(newShare.StockId, Convert.ToInt32(newShare.Price));

                shareOwnerControlClient.ApiShareOwnerCreateShareOwnershipByStockIdBySharesAmountPostAsync(
                    newShare.StockId, owner.UserId, newShare.Amount);

                OnRefresh();
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