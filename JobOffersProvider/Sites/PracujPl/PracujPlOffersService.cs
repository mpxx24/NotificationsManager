using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using JobOffersProvider.Common;
using JobOffersProvider.Common.Models;

namespace JobOffersProvider.Sites.PracujPl {
    public class PracujPlOffersService : IOffersService {
        private readonly IIndex<JobWebsiteTaskProviderType, IJobWebsiteTask> iindex;

        private readonly IJobWebsiteTask jobWebsiteTask;

        public PracujPlOffersService(IIndex<JobWebsiteTaskProviderType, IJobWebsiteTask> index) {
            this.iindex = index;
            this.jobWebsiteTask = this.iindex[JobWebsiteTaskProviderType.PracujPl];
        }

        public async Task<IEnumerable<JobModel>> GetOffers() {
            var jobOffers = await this.jobWebsiteTask.GetJobOffers();
            return jobOffers.ToList();
        }
    }
}