using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using JobOffersProvider.Common;
using JobOffersProvider.Common.Models;
using JobOffersProvider.Core;
using MPNotifier.Services.Contracts;

namespace MPNotifier.Services {
    public class ApplicationService : IApplicationService {
        private readonly IIndex<JobWebsiteTaskProviderType, IOffersService> iindex;

        private readonly IOffersService pracujPlOffersService;

        private readonly IOffersService trojmiastPlOffersService;

        public ApplicationService(IIndex<JobWebsiteTaskProviderType, IOffersService> index) {
            this.iindex = index;
            this.pracujPlOffersService = this.iindex[JobWebsiteTaskProviderType.PracujPl];
            this.trojmiastPlOffersService = this.iindex[JobWebsiteTaskProviderType.TrojmiastoPl];
        }

        public void PrepareApplicationData() {
            var offers = this.GetJobOffers();
            this.InitlializePseudoRepositoryContainer(offers);
        }

        private IEnumerable<JobModel> GetJobOffers() {
            var jobOffers = new List<JobModel>();

            var pracujPlJobOffers = this.pracujPlOffersService.GetOffers();
            var trojmiastoPlJobOffers = this.trojmiastPlOffersService.GetOffers();
            jobOffers.AddRange(pracujPlJobOffers);
            jobOffers.AddRange(trojmiastoPlJobOffers);

            return jobOffers;
        }


        private void InitlializePseudoRepositoryContainer(IEnumerable<JobModel> jobModels) {
            OffersContainer.JobOfferModels = jobModels.AsQueryable();
        }
    }
}