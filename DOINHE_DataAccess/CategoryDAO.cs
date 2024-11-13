using DOINHE_BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DOINHE_DataAccess
{
    public class CategoryDAO
    {
        public static List<Category> GetCategories()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Categories.ToList();
            }
        }

        public static Category GetCategoryById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Categories.FirstOrDefault(c => c.Id == id);
            }
        }

        public static void InsertCategory(Category category)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Categories.Add(category);
                context.SaveChanges();
            }
        }

        public static void UpdateCategory(Category category)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Categories.Update(category);
                context.SaveChanges();
            }
        }

        public static void DeleteCategory(Category category)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Categories.Remove(category);
                context.SaveChanges();
            }
        }
    }
}
