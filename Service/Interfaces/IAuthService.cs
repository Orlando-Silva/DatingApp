#region --Using--
using Core.Entities;
#endregion

namespace Service.Interfaces
{
    public interface IAuthService
    {
        User Register(User user, string password);
        User Login(string username, string password);
        bool UserExists(string username); 
    }
}