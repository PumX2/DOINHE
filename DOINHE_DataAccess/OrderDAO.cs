using DOINHE_BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DOINHE_DataAccess
{
    public class OrderDAO
    {
        public static List<Order> GetOrders()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Orders.Include(o => o.User).ToList();
            }
        }

        public static Order GetOrderById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Orders.Include(o => o.User).FirstOrDefault(o => o.Id == id);
            }
        }

        public static void InsertOrder(Order order)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Orders.Add(order);
                context.SaveChanges();
            }
        }

        public static void UpdateOrder(Order order)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Orders.Update(order);
                context.SaveChanges();
            }
        }

        public static void DeleteOrder(Order order)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Orders.Remove(order);
                context.SaveChanges();
            }
        }
    }
}
