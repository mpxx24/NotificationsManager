using System;
using System.Linq;
using System.Linq.Expressions;

namespace JobOffersProvider.Core {
    public interface IRepository<T> : IDisposable {
        IQueryable<T> GetAll();
        IQueryable<T> Filter(Expression<Func<T, bool>> func);
    }
}