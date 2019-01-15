#region --Using--
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
#endregion

namespace Core.Repository
{
    public interface IRepository<T> where T : class
    {
         T Add(T entity);
         IEnumerable<T> AddRange(IEnumerable<T> entities);
         
         void Remove(T entity);
         void RemoveRange(IEnumerable<T> entities);

        T Find(int id);
        T FirstOrDefault(Expression<Func<T,bool>> predicate);

        IEnumerable<T> All();
        IEnumerable<T> Where(Expression<Func<T,bool>> predicate);

    }
}