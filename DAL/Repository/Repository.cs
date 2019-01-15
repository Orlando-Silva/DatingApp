#region --Using--
using Core.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
#endregion

namespace DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region --Attributes--
        protected DbSet<T> Entity { get; private set; }

        private readonly DbContext _context;
        #endregion

        #region --Constructor--
        public Repository(DbContext context)
        {
            _context = context;
            Entity = _context.Set<T>();
        }
        #endregion

        #region --IRepository--
        public T Add(T entity) => Entity.Add(entity).Entity;
        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            Entity.AddRange(entities);
            return entities;
        }

        public void Remove(T entity) => Entity.Remove(entity);
        public void RemoveRange(IEnumerable<T> entities) => Entity.RemoveRange(entities);

        public IEnumerable<T> All() => Entity.ToList();
        public T Find(int id) => Entity.Find(id);
        public T FirstOrDefault(Expression<Func<T, bool>> predicate) => Entity.FirstOrDefault(predicate);
        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate) => Entity.Where(predicate);
        #endregion
    }
}
