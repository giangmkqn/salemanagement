using SalesWPFApp.Commands;
using SalesWPFApp.Views.Order;
using SalesWPFApp.Views.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SalesWPFApp.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged("Message"); }
        }

        private ContentControl currentPage;

        public ContentControl CurrentContent
        {
            get
            {
                return this.currentPage;
            }
            set { currentPage = value; OnPropertyChanged("CurrentContent"); }
        }

        private ContentControl _productView;
        private ContentControl _orderView;


        public ICommand BtnChangeViewCommand { get; set; }

        public MainWindowViewModel(ProductView productView, OrderView orderView)
        {
            CurrentContent = productView;
            _productView = productView;
            _orderView = orderView;
            BtnChangeViewCommand = new RelayCommand(ChangeView, CanChangeView);

        }

        private void ChangeView(object obj)
        {
            switch (obj.ToString())
            {
                case "product_view":
                    CurrentContent = _productView;
                    break;
                case "order_view":
                    CurrentContent = _orderView;
                    break;
            }
        }

        private bool CanChangeView(object obj)
        {
            return true;
        }
    }
}
