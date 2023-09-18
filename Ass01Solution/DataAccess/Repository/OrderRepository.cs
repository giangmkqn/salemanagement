using DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SaleManagementDbContext _context;
        public OrderRepository(SaleManagementDbContext saleManagementDbContext)
        {
            _context = saleManagementDbContext;
        }
        public int Create(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order.OrderId;
        }

        public void Delete(int orderId)
        {
            var orderDetail = _context.OrderDetails.Where(x => x.OrderId == orderId).ToList();
            _context.OrderDetails.RemoveRange(orderDetail);
            var order = _context.Orders.FirstOrDefault(p => p.OrderId == orderId);
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }

        public List<Order> GetAll()
        {
            return _context.Orders.Include(x => x.OrderDetails).ToList();
        }

        public void Update(Order order)
        {
            var orderToUpdate = _context.Orders.FirstOrDefault(x => x.OrderId == order.OrderId);
            orderToUpdate.OrderDate = order.OrderDate;
            orderToUpdate.RequiredDate = order.RequiredDate;
            orderToUpdate.ShippedDate = order.ShippedDate;
            orderToUpdate.Freight = order.Freight;
            orderToUpdate.MemberId = order.MemberId;
            _context.SaveChanges();
        }
    }
}
