using System.Data;
using System.Data.Entity;
using Data.Interfaces;

namespace Data.EntityFramework
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly Func<DbContext> _factory;

        public Repository(Func<DbContext> factory)
        {
            _factory = factory;
        }

        protected DbContext Context
        {
            get { return _factory(); }
        }

        protected DbSet<T> Set
        {
            get { return _factory().Set<T>(); }
        }

        public virtual void Create(T entity)
        {
            Set.Add(entity);
        }

        public virtual void Update(T entity)
        {
            Set.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual T FindById(object id)
        {
            return Set.Find(id);
        }

        public virtual IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return Set.Where(predicate);
        }

        public virtual IQueryable<T> GetAll()
        {
            return Set;
        }

        public virtual void Delete(T entity)
        {
            Set.Remove(entity);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            Find(predicate).ToList().ForEach(e => Set.Remove(e));
        }

        public virtual IQueryable<T> ExecuteSql(string sql, params object[] parameters)
        {
            return Set.SqlQuery(sql, parameters).AsQueryable();
        }
    }
}
