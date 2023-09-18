using DataAccess.Entity;
using DataAccess.Models;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObject.Bussiness
{
    public class OrderObject : IOrderObject
    {
        private IOrderRepository _orderRepository;
        public OrderObject(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public int CreateOrder(OrderDAO orderDAO)
        {
            var order = new Order()
            {
                MemberId = orderDAO.MemberId,
                OrderDate = orderDAO.OrderDate,
                RequiredDate = orderDAO.RequiredDate,
                ShippedDate = orderDAO.ShippedDate,
                Freight = orderDAO.Freight
            };
            return _orderRepository.Create(order);
        }

        public void Delete(int orderId)
        {
            _orderRepository.Delete(orderId);
        }

        public List<OrderDAO> GetAll()
        {
            var orders = _orderRepository.GetAll();

            return orders.Select(x => new OrderDAO()
            {
                OrderId = x.OrderId,
                MemberId = x.MemberId,
                OrderDate = x.OrderDate,
                RequiredDate = x.RequiredDate,
                ShippedDate = x.ShippedDate,
                Freight = x.Freight,
                OrderDetailDAOs = new ObservableCollection<OrderDetailDAO>(x.OrderDetails.Select(o => new OrderDetailDAO()
                {
                    ProductId = o.ProductId,
                    OrderId = o.OrderId,
                    UnitPrice = o.UnitPrice,
                    Quantity = o.Quantity,
                    Discount = o.Discount
                })),
            }).ToList();
        }

        public void Update(OrderDAO modifyOrder)
        {
            var order = new Order()
            {
                OrderId = modifyOrder.OrderId,
                MemberId = modifyOrder.MemberId,
                OrderDate = modifyOrder.OrderDate,
                RequiredDate = modifyOrder.RequiredDate,
                ShippedDate = modifyOrder.ShippedDate,
                Freight = modifyOrder.Freight,
            };
            _orderRepository.Update(order);
        }
        
    }
}
