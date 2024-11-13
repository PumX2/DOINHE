using DOINHE_BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DOINHE_DataAccess
{
    public class UserDAO
    {
        public static List<User> GetUsers()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Users.ToList();
            }
        }

        public static User GetUserById(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Users.FirstOrDefault(u => u.Id == id);
            }
        }

        public static void InsertUser(User user)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public static void UpdateUser(User user)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Users.Update(user);
                context.SaveChanges();
            }
        }

        public static void DeleteUser(User user)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }
    }
}
