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

        private readonly IRepository<JobModel> repository;

        public ApplicationService(IIndex<JobWebsiteTaskProviderType, IOffersService> index, IRepository<JobModel> repository) {
            this.iindex = index;
            this.repository = repository;
            this.pracujPlOffersService = this.iindex[JobWebsiteTaskProviderType.PracujPl];
            this.trojmiastPlOffersService = this.iindex[JobWebsiteTaskProviderType.TrojmiastoPl];
        }

        public void PrepareApplicationData() {
            if (this.repository.GetAll() != null) {
                return;
            }

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
            var models = jobModels.AsQueryable();
            this.repository.SetContext(models);
        }
    }
}