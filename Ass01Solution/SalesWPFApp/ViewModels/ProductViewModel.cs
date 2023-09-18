using BussinessObject.Repository;
using DataAccess.Models;
using SalesWPFApp.Commands;
using SalesWPFApp.Views.Product;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SalesWPFApp.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool showWindows = true;
        #endregion


        #region declare object
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

        private ObservableCollection<ProductDAO> productList;
        public ObservableCollection<ProductDAO> ProductList
        {
            get { return productList; }
            set { productList = value; OnPropertyChanged("ProductList"); }
        }

        private string _statusMessage;

        public string StatusMessage
        {
            get { return _statusMessage; }
            set { _statusMessage = value; this.OnPropertyChanged("StatusMessage"); }
        }
        #endregion

        #region constructor
        private IProductObject _productObject;
        private AddProductView _addProductView;
        public ICommand ShowCreateProductPopupCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }
        public ICommand DatagridSelected { get; set; }
        public string BoxCaption = "Product Management";

        public ProductViewModel(IProductObject productObject)
        {
            _productObject = productObject;
            LoadData();
            ShowCreateProductPopupCommand = new RelayCommand(ShowWindow, CanShowWindow);
            DeleteProductCommand = new RelayCommand(DeleteProduct, CanDeleteProduct);
        }
        #endregion


        #region DisplayOperation

        private void LoadData()
        {
            productList = new ObservableCollection<ProductDAO>(_productObject.GetAll());
        }

        //create new or update status

        public void HandleProductSuccess(ProductDAO actionProduct)
        {
            if(StatusButton.ToLower() == "update")
            {
                productList[SelectedIndex] = actionProduct;
                return;
            }

            productList.Add(actionProduct);
        }

        private void DeleteProduct(object obj)
        {
            if (SelectedIndex < 0)
            {
                MessageBox.Show("Please select a product", BoxCaption, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Do you want to delete this product", BoxCaption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;
            
            var productToDelete = productList.ElementAt(SelectedIndex);
            _productObject.Delete(productToDelete.ProductId);
            productList.RemoveAt(SelectedIndex);
            MessageBox.Show("Deleted Successfully", BoxCaption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool CanDeleteProduct(object obj)
        {
            return true;
        }

        private void ShowWindow(object obj)
        {
            //create or update product
            StatusButton = obj.ToString();
            if (StatusButton.ToLower() == "update" && SelectedIndex < 0)
            {
                MessageBox.Show("Please select a product", BoxCaption, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _addProductView = new AddProductView();
            _addProductView.DataContext = new AddProductViewModel(this, _productObject);
            _addProductView.ShowDialog();
        }

        private bool CanShowWindow(object obj)
        {
            return showWindows;
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
        #endregion
    }
}
