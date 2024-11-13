using DOINHE_BusinessObject;
using System.Collections.Generic;

namespace DOINHE_Repository
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User GetUserById(int id);
        void SaveUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}
