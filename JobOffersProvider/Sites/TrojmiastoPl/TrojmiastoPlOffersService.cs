using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using JobOffersProvider.Common;
using JobOffersProvider.Common.Models;

namespace JobOffersProvider.Sites.TrojmiastoPl {
    public class TrojmiastoPlOffersService : IOffersService {
        private readonly IIndex<JobWebsiteTaskProviderType, IJobWebsiteTask> iindex;

        private readonly IJobWebsiteTask jobWebsiteTask;

        public TrojmiastoPlOffersService(IIndex<JobWebsiteTaskProviderType, IJobWebsiteTask> index) {
            this.iindex = index;
            this.jobWebsiteTask = this.iindex[JobWebsiteTaskProviderType.TrojmiastoPl];
        }

        public async Task<IEnumerable<JobModel>> GetOffers() {
            var jobOffers = await this.jobWebsiteTask.GetJobOffers();
            return jobOffers.ToList();
        }
    }
}