using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using JobOffersProvider.Common;
using JobOffersProvider.Common.Models;

namespace JobOffersProvider.Sites.TrojmiastoPl {
    public class TrojmiastoPlOffersService : IOffersService {
        private readonly IIndex<JobWebsiteTaskProviderType, IJobWebsiteTask> iJobWebsiteTaskIndex;

        private readonly IJobWebsiteTask jobWebsiteTask;

        public TrojmiastoPlOffersService(IIndex<JobWebsiteTaskProviderType, IJobWebsiteTask> index) {
            this.iJobWebsiteTaskIndex = index;
            this.jobWebsiteTask = this.iJobWebsiteTaskIndex[JobWebsiteTaskProviderType.TrojmiastoPl];
        }

        public IEnumerable<JobModel> GetOffers() {
            var jobOffers = this.jobWebsiteTask.GetJobOffers();
            return jobOffers.Result.ToList();
        }
    }
}