using BussinessObject.Bussiness;
using BussinessObject.Repository;
using DataAccess.Entity;
using DataAccess.Models;
using SalesWPFApp.Commands;
using SalesWPFApp.ValidationRules;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace SalesWPFApp.ViewModels
{
    public class AddOrderViewModel : ViewModelBase, INotifyDataErrorInfo
    {

        #region declare error validation
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool Cancreate => !HasErrors;

        public bool HasErrors => _errorsByPropertyName.Any();

        public IEnumerable? GetErrors(string? propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ?
                        _errorsByPropertyName[propertyName] : null;
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(CanCreate));
        }

        private void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
        #endregion

        #region declare properties

        private string textAction;
        public string TextAction
        {
            get => textAction;
            set
            {
                textAction = value;
                OnPropertyChanged(nameof(TextAction));
            }
        }

        private int orderId;
        public int OrderId
        {
            get { return orderId; }
            set { orderId = value; OnPropertyChanged("OrderId"); }
        }

        private int memberId;
        public int MemberId
        {
            get { return memberId; }
            set { memberId = value; OnPropertyChanged("MemberId"); }
        }

        private DateTime? orderDate;
        public DateTime? OrderDate
        {
            get { return orderDate; }
            set { orderDate = value; OnPropertyChanged("OrderDate"); }
        }

        private DateTime? requiredDate;
        public DateTime? RequiredDate
        {
            get { return requiredDate; }
            set { requiredDate = value; OnPropertyChanged("RequiredDate"); }
        }

        private DateTime? shippedDate;
        public DateTime? ShippedDate
        {
            get { return shippedDate; }
            set { shippedDate = value; OnPropertyChanged("ShippedDate"); }
        }

        private decimal? freight;
        public decimal? Freight
        {
            get { return freight; }
            set { freight = value; OnPropertyChanged("Freight"); }
        }


        private ObservableCollection<OrderDetailDAO> orderDetailDAOs;
        public ObservableCollection<OrderDetailDAO> OrderDetailDAOs
        {
            get { return orderDetailDAOs; }
            set { orderDetailDAOs = value; OnPropertyChanged("OrderDetailDAOs"); }
        }
        public bool showWindows = true;
        #endregion

        #region contructor
        public ICommand SaveOrderCommand { get; private set; }
        private OrderViewModel _orderViewModel;
        private IOrderObject _orderObject;
        public AddOrderViewModel(OrderViewModel orderViewModel, IOrderObject orderObject)
        {
            SaveOrderCommand = new RelayCommand(SaveOrder, CanShowWindow);
            _orderViewModel = orderViewModel;
            _orderObject = orderObject;
            TextAction = orderViewModel.StatusButton + " Order";

            if (orderViewModel.StatusButton.ToLower() == "update")
            {
                var productToModify = orderViewModel.OrderList.ElementAt(orderViewModel.SelectedIndex);
                OrderId = productToModify.OrderId;
                MemberId = productToModify.MemberId;
                OrderDate = productToModify.OrderDate;
                RequiredDate = productToModify.RequiredDate;
                ShippedDate = productToModify.ShippedDate;
                Freight = productToModify.Freight;
            }
            else
            {
                MemberId = 1; //default
            }
        }
        #endregion

        #region function
        private void SaveOrder(object obj)
        {
            ValidationAllField();
            if (!Cancreate) return;

            var order = new OrderDAO()
            {
                OrderId = OrderId,
                MemberId = MemberId,
                OrderDate = OrderDate ?? DateTime.Now,
                RequiredDate = RequiredDate,
                ShippedDate = ShippedDate,
                Freight = Freight
            };

            //handle add new product
            if (_orderViewModel.StatusButton.ToLower() == "create")
            {
                order.OrderId = _orderObject.CreateOrder(order);
                if (order.OrderId <= 0)
                {
                    MessageBox.Show("Add fail", "Product Management", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }
                MessageBox.Show($"Added Successfully", "Product Management", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }

            //handle modify prodduct
            if (_orderViewModel.StatusButton.ToLower() == "update")
            {
                order.OrderId = OrderId;
                _orderObject.Update(order);
                MessageBox.Show($"Updated Successfully", "Product Management", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);

            }

            _orderViewModel.HandleProductSuccess(order);
            var form = (Window)obj;
            form.Close();
        }

        public void ValidationAllField()
        {
            //ClearErrors(nameof(ProductName));
            //ClearErrors(nameof(Weight));
            //ClearErrors(nameof(UnitPrice));
            //ClearErrors(nameof(UnitsInStock));

            //ProductValidationRule.ValidateProductName(ProductName).ForEach(x => AddError(nameof(ProductName), x));
            //ProductValidationRule.ValidateWeigt(Weight).ForEach(x => AddError(nameof(Weight), x));
            //ProductValidationRule.ValidateUnitPrice(UnitPrice).ForEach(x => AddError(nameof(UnitPrice), x));
            //ProductValidationRule.ValidateUnitsInStockValidate(UnitsInStock).ForEach(x => AddError(nameof(UnitsInStock), x));

        }

        private bool CanShowWindow(object obj)
        {
            return showWindows;
        }


        #endregion
    }
}
