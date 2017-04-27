using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using JobOffersProvider.Common;
using JobOffersProvider.Common.Models;

namespace JobOffersProvider.Sites.PracujPl {
    public class PracujPlOffersService : IOffersService {
        private readonly IIndex<JobWebsiteTaskProviderType, IJobWebsiteTask> iJobWebsiteTaskIndex;

        private readonly IJobWebsiteTask jobWebsiteTask;

        public PracujPlOffersService(IIndex<JobWebsiteTaskProviderType, IJobWebsiteTask> index) {
            this.iJobWebsiteTaskIndex = index;
            this.jobWebsiteTask = this.iJobWebsiteTaskIndex[JobWebsiteTaskProviderType.PracujPl];
        }

        public IEnumerable<JobModel> GetOffers() {
            var jobOffers = this.jobWebsiteTask.GetJobOffers();
            return jobOffers.Result.ToList();
        }
    }
}