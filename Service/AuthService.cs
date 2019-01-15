#region --Using--
using Core;
using Core.Entities;
using Service.Interfaces;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
#endregion

namespace Service
{
    public class AuthService : Service, IAuthService
    {

        public AuthService(IUnityOfWork unityOfWork) : base(unityOfWork)
        {

        }

        #region --IAuthService--
        public User Register(User user, string password)
        {
            user = CreatePasswordHash(user, password);
            UnityOfWork.Users.Add(user);
            UnityOfWork.SaveChanges();
            return user;
        }

        public User Login(string username, string password)
        {
            var user = UnityOfWork.Users.FirstOrDefault(_ => _.Name == username);

            if (user is null)
                return null;

            var passwordMatches = VerifyPasswordHash(password, user.PasswordSalt, user.PasswordHash);

            return passwordMatches 
                    ? user 
                    : null;

        }

        public bool UserExists(string username) => UnityOfWork.Users.Where(_ => _.Name == username).Any();

        private User CreatePasswordHash(User user, string password)
        {
            using (var hmac = new HMACSHA512())
            {
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
            return user;
        }
        #endregion

        private bool VerifyPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
