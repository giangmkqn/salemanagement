using BussinessObject.Bussiness;
using BussinessObject.Repository;
using DataAccess.Models;
using SalesWPFApp.Commands;
using SalesWPFApp.Views.Order;
using SalesWPFApp.Views.Product;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SalesWPFApp.ViewModels
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region declare object
        private bool showWindows = true;
        public string StatusButton { get; set; }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        private ObservableCollection<OrderDAO> orderList;
        public ObservableCollection<OrderDAO> OrderList
        {
            get { return orderList; }
            set { orderList = value; OnPropertyChanged("OrderList"); }
        }

        private string _statusMessage;

        public string StatusMessage
        {
            get { return _statusMessage; }
            set { _statusMessage = value; this.OnPropertyChanged("StatusMessage"); }
        }
        #endregion

        #region constructor
        private IOrderObject _orderObject;
        private AddOrderView _addOrderView;
        public ICommand ShowCreateOrderPopupCommand { get; set; }
        public ICommand DeleteOrderCommand { get; set; }
        public ICommand DatagridSelected { get; set; }
        public string BoxCaption = "Order Management";

        public OrderViewModel(IOrderObject orderObject)
        {
            _orderObject = orderObject;
            LoadData();
            ShowCreateOrderPopupCommand = new RelayCommand(ShowWindow, CanShowWindow);
            DeleteOrderCommand = new RelayCommand(DeleteOrder, CanDeleteOrder);
        }
        #endregion

        private void LoadData()
        {
            orderList = new ObservableCollection<OrderDAO>(_orderObject.GetAll());
        }

        public void HandleProductSuccess(OrderDAO actionOrder)
        {
            if (StatusButton.ToLower() == "update")
            {
                orderList[SelectedIndex] = actionOrder;
                return;
            }

            orderList.Add(actionOrder);
        }

        private void DeleteOrder(object obj)
        {
            if (SelectedIndex < 0)
            {
                MessageBox.Show("Please select a order", BoxCaption, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Do you want to delete this order", BoxCaption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;

            var productToDelete = orderList.ElementAt(SelectedIndex);
            _orderObject.Delete(productToDelete.OrderId);
            orderList.RemoveAt(SelectedIndex);
            MessageBox.Show("Deleted Successfully", BoxCaption, MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private bool CanDeleteOrder(object obj)
        {
            return true;
        }

        private void ShowWindow(object obj)
        {
            //create or update order
            StatusButton = obj.ToString();
            if(StatusButton.ToLower() == "update" && SelectedIndex < 0)
            {
                MessageBox.Show("Please select a order", BoxCaption, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _addOrderView = new AddOrderView();
            _addOrderView.DataContext = new AddOrderViewModel(this, _orderObject);
            _addOrderView.ShowDialog();
        }

        private bool CanShowWindow(object obj)
        {
            return showWindows;
        }
    }
}
