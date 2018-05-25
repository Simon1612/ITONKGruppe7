using System.ComponentModel;
using System.Runtime.CompilerServices;
using TradeClient.ViewModels;

namespace TradeClient.Helpers
{
    //Klasse udviklet i forbindelse med tidligere opgave
    public class ViewModelLocator : INotifyPropertyChanged
    {
        private static ViewModelLocator _instance;

        public static ViewModelLocator Instance => _instance ?? (_instance = new ViewModelLocator());

        private BaseViewModel _viewmodel;
        public BaseViewModel ViewModel
        {
            get
            {
                return _viewmodel;
            }
            set
            {
                _viewmodel = value;
                Notify();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify([CallerMemberName]string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}