using DataAccess.Entity;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IProductRepository
    {
        public int Create(Product product);
        public List<Product> GetAll();
        public void Delete(int productId);
        public void Update(Product product);
    }
}
