using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IOrderRepository
    {
        public int Create(Order product);
        public List<Order> GetAll();
        public void Delete(int orderId);
        public void Update(Order order);
    }
}
