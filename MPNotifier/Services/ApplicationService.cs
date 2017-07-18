using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using JobOffersProvider.Common;
using JobOffersProvider.Common.Models;
using JobOffersProvider.Core;
using MPNotifier.Models;
using MPNotifier.Services.Contracts;

namespace MPNotifier.Services {
    public class ApplicationService : IApplicationService {
        private readonly IIndex<WebsiteType, IOffersService> iindex;

        private readonly IOffersService pracujPlOffersService;

        private readonly IOffersService trojmiastPlOffersService;

        private readonly IRepository<JobModel> repository;

        public ApplicationService(IIndex<WebsiteType, IOffersService> index, IRepository<JobModel> repository) {
            this.iindex = index;
            this.repository = repository;
            this.pracujPlOffersService = this.iindex[WebsiteType.PracujPl];
            this.trojmiastPlOffersService = this.iindex[WebsiteType.TrojmiastoPl];
        }

        public void PrepareApplicationData(ApplicationSettingsModel settings) {
            if (this.repository.GetAll() != null) {
                return;
            }

            var offers = this.GetJobOffers(settings);
            this.InitlializePseudoRepositoryContainer(offers);
        }

        private IEnumerable<JobModel> GetJobOffers(ApplicationSettingsModel settings) {
            var jobOffers = new List<JobModel>();
            var searchModel = this.ConvertApplicationSettingsModelToSearchSettingsModel(settings);

            switch (searchModel.Website) {
                case WebsiteType.PracujPl:
                    var pracujPlJobOffers = this.pracujPlOffersService.GetOffers(searchModel.Text);
                    jobOffers.AddRange(pracujPlJobOffers);
                    break;
                case WebsiteType.TrojmiastoPl:
                    var trojmiastoPlJobOffers = this.trojmiastPlOffersService.GetOffers(searchModel.Text);
                    jobOffers.AddRange(trojmiastoPlJobOffers);
                    break;
                case WebsiteType.Undefined:
                    break;
            }

            return jobOffers;
        }

        private void InitlializePseudoRepositoryContainer(IEnumerable<JobModel> jobModels) {
            var models = jobModels.AsQueryable();
            this.repository.SetContext(models);
        }

        private SearchSettingsModel ConvertApplicationSettingsModelToSearchSettingsModel(ApplicationSettingsModel model) {
            return new SearchSettingsModel {
                Text = model.SearchText,
                OfferType = model.OfferType,
                Website = model.Website
            };
        }
    }
}