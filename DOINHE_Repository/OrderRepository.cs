using DOINHE_BusinessObject;
using DOINHE_DataAccess;
using System.Collections.Generic;

namespace DOINHE_Repository
{
    public class OrderRepository : IOrderRepository
    {
        public List<Order> GetAllOrders() => OrderDAO.GetOrders();
        public Order GetOrderById(int id) => OrderDAO.GetOrderById(id);
        public void SaveOrder(Order order) => OrderDAO.InsertOrder(order);
        public void UpdateOrder(Order order) => OrderDAO.UpdateOrder(order);
        public void DeleteOrder(Order order) => OrderDAO.DeleteOrder(order);
    }
}
