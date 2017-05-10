using System;
using System.Linq;
using System.Linq.Expressions;
using JobOffersProvider.Common.Models;

namespace JobOffersProvider.Core {
    public class JobModelRepository : IRepository<JobModel> {
        private static IQueryable<JobModel> context;

        public void Dispose() {
            context = null;
        }

        public IQueryable<JobModel> GetAll() {
            return context;
        }

        public IQueryable<JobModel> Filter(Expression<Func<JobModel, bool>> func) {
            return context.Where(func).AsQueryable();
        }

        public void SetContext(IQueryable<JobModel> data) {
            context = data;
        }
    }
}