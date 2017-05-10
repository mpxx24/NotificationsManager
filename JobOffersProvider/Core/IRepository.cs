using System;
using System.Linq;
using System.Linq.Expressions;
using JobOffersProvider.Common.Models;

namespace JobOffersProvider.Core {
    public interface IRepository<T> : IDisposable {
        IQueryable<T> GetAll();
        IQueryable<T> Filter(Expression<Func<T, bool>> func);
        void SetContext(IQueryable<JobModel> context);
    }
}