using System;
using System.Linq;
using System.Linq.Expressions;
using JobOffersProvider.Common.Models;

namespace JobOffersProvider.Core {
    public class JobModelRepository : IRepository<JobModel> {
        private IQueryable<JobModel> context;

        public JobModelRepository() {
            if (OffersContainer.JobOfferModels == null) {
                //TODO: log
            }

            this.context = OffersContainer.JobOfferModels;
        }

        public void Dispose() {
            this.context = null;
        }

        public IQueryable<JobModel> GetAll() {
            return this.context;
        }

        public IQueryable<JobModel> Filter(Expression<Func<JobModel, bool>> func) {
            return this.context.Where(func).AsQueryable();
        }
    }
}