namespace Data.EntityFramework
{
    using Interfaces;
    using System;
    using System.Data.Entity;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly Func<DbContext> _factory;

        public UnitOfWork(Func<DbContext> factory)
        {
            _factory = factory;
        }

        public void Commit()
        {
            _factory().SaveChanges();
        }
    }
}
