using DOINHE_BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DOINHE_DataAccess
{
    public class ProductDAO
    {
        public static List<Product> GetProducts()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Products.Include(p => p.Categories).ToList();
            }
        }

        public static Product GetProductById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Products.Include(p => p.Categories).FirstOrDefault(p => p.Id == id);
            }
        }

        public static void InsertProduct(Product product)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Products.Add(product);
                context.SaveChanges();
            }
        }

        public static void UpdateProduct(Product product)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Products.Update(product);
                context.SaveChanges();
            }
        }

        public static void DeleteProduct(Product product)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Products.Remove(product);
                context.SaveChanges();
            }
        }
    }
}
