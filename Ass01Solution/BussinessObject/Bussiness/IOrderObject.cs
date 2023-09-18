using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Bussiness
{
    public interface IOrderObject
    {
        public int CreateOrder(OrderDAO order);
        public List<OrderDAO> GetAll();
        public void Delete(int orderId);
        public void Update(OrderDAO order);
    }
}
