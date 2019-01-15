#region --Using--
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
#endregion

namespace Core.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> IncludePhotos();
        IEnumerable<User> IncludePhotos(Expression<Func<User,bool>> predicate);
    }
}