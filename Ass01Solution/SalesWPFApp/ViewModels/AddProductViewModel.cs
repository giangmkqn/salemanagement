using BussinessObject.Repository;
using DataAccess.Models;
using SalesWPFApp.Commands;
using SalesWPFApp.ValidationRules;
using SalesWPFApp.Views.Product;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SalesWPFApp.ViewModels
{
    public class AddProductViewModel : ViewModelBase, INotifyDataErrorInfo
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

        private int productId;
        public int ProductId
        {
            get { return productId; }
            set
            {
                productId = value;
                OnPropertyChanged(nameof(ProductId));
            }
        }

        private string productName;
        public string ProductName
        {
            get { return productName; }
            set { productName = value;

                //validate product name
                ClearErrors(nameof(ProductName));
                var validateProductName = ProductValidationRule.ValidateProductName(productName);
                if (validateProductName.Any())
                {
                    validateProductName.ForEach(x => AddError(nameof(ProductName), x));
                }

                OnPropertyChanged(nameof(ProductName)); }
        }

        private string weight;
        public string Weight
        {
            get { return weight; }
            set { weight = value;

                //validate weight
                ClearErrors(nameof(Weight));
                var validateWeigt = ProductValidationRule.ValidateWeigt(weight);
                if (validateWeigt.Any())
                {
                    validateWeigt.ForEach(x => AddError(nameof(Weight), x));
                }

                OnPropertyChanged(nameof(Weight)); }
        }

        private decimal? unitPrice;
        public decimal? UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value;

                //validate UnitPrice
                ClearErrors(nameof(UnitPrice));
                var validateUnitPrice = ProductValidationRule.ValidateUnitPrice(unitPrice);
                if (validateUnitPrice.Any())
                {
                    validateUnitPrice.ForEach(x => AddError(nameof(UnitPrice), x));
                }

                OnPropertyChanged("UnitPrice"); }
        }

        private int? unitsInStock;
        public int? UnitsInStock
        {
            get { return unitsInStock; }
            set { unitsInStock = value;

                //validate UnitsInStock
                ClearErrors(nameof(UnitsInStock));
                var validateUnitsInStockValidate = ProductValidationRule.ValidateUnitsInStockValidate(unitsInStock);
                if (validateUnitsInStockValidate.Any())
                {
                    validateUnitsInStockValidate.ForEach(x => AddError(nameof(UnitsInStock), x));
                }
                OnPropertyChanged("UnitsInStock"); }
        }

        public bool showWindows = true;
        #endregion


        #region contructor
        public ICommand SaveProductCommand { get; private set; }
        private ProductViewModel _productViewModel;
        private IProductObject _productObject;
        public AddProductViewModel(ProductViewModel productViewModel, IProductObject productObject)
        {
            SaveProductCommand = new RelayCommand(SaveProduct, CanShowWindow);
            _productViewModel = productViewModel;
            _productObject = productObject;
            TextAction = productViewModel.StatusButton + " Product";

            if(productViewModel.StatusButton.ToLower() == "update")
            {
                var productToModify = productViewModel.ProductList.ElementAt(productViewModel.SelectedIndex);
                ProductId = productToModify.ProductId;
                ProductName = productToModify.ProductName;
                Weight = productToModify.Weight;
                UnitPrice = productToModify.UnitPrice;
                UnitsInStock = productToModify.UnitsInStock;
            }
        }

        #endregion

        #region function
        private void SaveProduct(object obj)
        {
            ValidationAllField();
            if (!Cancreate) return;

            var product = new ProductDAO()
            {
                ProductName = ProductName,
                Weight = Weight,
                UnitPrice = UnitPrice ?? 0,
                UnitsInStock = UnitsInStock ?? 0,
            };

            //handle add new product
            if(_productViewModel.StatusButton.ToLower() == "create")
            {
                product.ProductId = _productObject.CreateProduct(product);
                if (product.ProductId <= 0)
                {
                    MessageBox.Show("Add fail", "Product Management", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    return;
                }
                MessageBox.Show($"Added Successfully", "Product Management", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }

            //handle modify prodduct
            if (_productViewModel.StatusButton.ToLower() == "update")
            {
                product.ProductId = ProductId;
                _productObject.Update(product);
                MessageBox.Show($"Updated Successfully", "Product Management", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);

            }

            _productViewModel.HandleProductSuccess(product);
            var form = (Window)obj;
            form.Close();
        }

        public void ValidationAllField()
        {
            ClearErrors(nameof(ProductName));
            ClearErrors(nameof(Weight));
            ClearErrors(nameof(UnitPrice));
            ClearErrors(nameof(UnitsInStock));

            ProductValidationRule.ValidateProductName(ProductName).ForEach(x => AddError(nameof(ProductName), x));
            ProductValidationRule.ValidateWeigt(Weight).ForEach(x => AddError(nameof(Weight), x));
            ProductValidationRule.ValidateUnitPrice(UnitPrice).ForEach(x => AddError(nameof(UnitPrice), x));
            ProductValidationRule.ValidateUnitsInStockValidate(UnitsInStock).ForEach(x => AddError(nameof(UnitsInStock), x));

        }

        private bool CanShowWindow(object obj)
        {
            return showWindows;
        }


        #endregion



        #region DisplayOperation

        #endregion
    }
}
