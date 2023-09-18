using DataAccess.Entity;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly SaleManagementDbContext _context;
        public ProductRepository(SaleManagementDbContext saleManagementDbContext) { 
        _context = saleManagementDbContext;
        }
        public int Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product.ProductId;
        }

        public void Delete(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public List<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public void Update(Product product)
        {
            var productToUpdate = _context.Products.FirstOrDefault(x => x.ProductId == product.ProductId);
            productToUpdate.ProductName = product.ProductName;
            productToUpdate.Weight = product.Weight;
            productToUpdate.CategoryId = product.CategoryId;
            productToUpdate.UnitPrice = product.UnitPrice;
            productToUpdate.UnitsInStock = product.UnitsInStock;
            
            _context.SaveChanges();
        }
    }
}
