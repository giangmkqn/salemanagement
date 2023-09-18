using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Repository
{
    public interface IProductObject
    {
        public int CreateProduct(ProductDAO product);
        public List<ProductDAO> GetAll();
        public void Delete(int productId);
        public void Update(ProductDAO product);

    }
}
