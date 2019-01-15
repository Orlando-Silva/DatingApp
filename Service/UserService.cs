#region --Using--
using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Entities;
using Service.Interfaces;
#endregion

namespace Service
{
    public class UserService : Service, IUserService
    {

        public UserService(IUnityOfWork unityOfWork) : base(unityOfWork)
        {
        }

        #region --IUserService--
        public IList<User> GetUsers()
        {
            return UnityOfWork
                .Users
                .IncludePhotos()
                .ToList()
                ?? throw new ArgumentNullException("There are no users.");
        }

        public User GetUser(int id)
        {
            return UnityOfWork
                .Users
                .IncludePhotos(_ => _.Id == id)
                .FirstOrDefault()
                ?? throw new ArgumentNullException($"Cannot find user. ID: { id }.");
        }
        #endregion
    }
}
