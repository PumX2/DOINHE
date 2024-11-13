using DOINHE_BusinessObject;
using DOINHE_DataAccess;
using System.Collections.Generic;

namespace DOINHE_Repository
{
    public class UserRepository : IUserRepository
    {
        public List<User> GetAllUsers() => UserDAO.GetUsers();
        public User GetUserById(int id) => UserDAO.GetUserById(id);
        public void SaveUser(User user) => UserDAO.InsertUser(user);
        public void UpdateUser(User user) => UserDAO.UpdateUser(user);
        public void DeleteUser(User user) => UserDAO.DeleteUser(user);
    }
}
