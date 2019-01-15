#region --Using--
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Entities;
using Core.Repository;
using DAL.Context;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DAL.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DatingAppContext context) : base(context)
        {
        }



        #region --IUserRepository--
        public IEnumerable<User> IncludePhotos() => Entity.Include(_ => _.Photos).ToList();

        public IEnumerable<User> IncludePhotos(Expression<Func<User, bool>> predicate) => Entity.Include(_ => _.Photos).Where(predicate);
        #endregion
    }
}
