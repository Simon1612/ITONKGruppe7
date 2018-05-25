using System.Windows;
using TradeClient.ViewModels;
using TradeClient.Helpers;

namespace TradeClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BaseViewModel _mainWindowViewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();

            ViewModelLocator.Instance.ViewModel = _mainWindowViewModel;
            DataContext = _mainWindowViewModel;
        }

        private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
