#region --Using--
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Core;
using Core.Entities;
using Newtonsoft.Json;
#endregion

namespace DatingApp.API.Data
{
    public class Seed
    {
        private readonly IUnityOfWork _unityOfWork;
        
        public Seed(IUnityOfWork unityOfWork)
        {
            _unityOfWork = unityOfWork;
        }

        public void SeedUsers() 
        {
            var userData =  File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            foreach (var user in users)
            {
                var userWithPassword = CreatePasswordHash(user, "password");
                _unityOfWork.Users.Add(user);
            }

            _unityOfWork.SaveChanges(); 
        }

        private User CreatePasswordHash(User user, string password)
        {
            using(var hmac = new HMACSHA512()) 
            {
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));   
            }
            return user;
        }
    }
}