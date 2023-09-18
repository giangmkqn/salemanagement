using DataAccess.Entity;
using DataAccess.Models;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Repository
{
    public class ProductObject : IProductObject
    {
        private IProductRepository _productRepository;
        public ProductObject(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public int CreateProduct(ProductDAO productDAO)
        {
            var product = new Product()
            {
                ProductName = productDAO.ProductName,
                CategoryId = 1, //default
                Weight = productDAO.Weight,
                UnitPrice = productDAO.UnitPrice,
                UnitsInStock = productDAO.UnitsInStock
            };
            return _productRepository.Create(product);
        }

        public void Delete(int productId)
        {
            _productRepository.Delete(productId); 
        }

        public List<ProductDAO> GetAll()
        {
           var products = _productRepository.GetAll();

           return products.Select(x => new ProductDAO()
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                Weight = x.Weight,
            }).ToList();
        }

        public void Update(ProductDAO modifyProduct)
        {
            var product = new Product()
            {
                ProductId = modifyProduct.ProductId,
                ProductName = modifyProduct.ProductName,
                UnitPrice = modifyProduct.UnitPrice, 
                CategoryId = 1, //default
                Weight= modifyProduct.Weight,
                UnitsInStock = modifyProduct.UnitsInStock,
            };
            _productRepository.Update(product);
        }
    }
}
