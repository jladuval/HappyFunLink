namespace Data.Interfaces
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IRepository<T>
    {
        void Create(T entity);
        void Update(T entity);
        T FindById(Object id);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll();
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> predicate);
        IQueryable<T> ExecuteSql(string sql, params object[] parameters);
    }

}
