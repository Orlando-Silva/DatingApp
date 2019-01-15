using Core.Entities;
using System.Collections.Generic;

namespace Service.Interfaces
{
    public interface IUserService
    {
        IList<User> GetUsers();
        User GetUser(int id);
    }
}
