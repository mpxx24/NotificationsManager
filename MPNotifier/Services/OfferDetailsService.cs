using System;
using System.Linq;
using Autofac.Features.Indexed;
using JobOffersProvider.Common;
using JobOffersProvider.Common.Models;
using JobOffersProvider.Core;
using MPNotifier.Models.ViewModels;
using MPNotifier.Services.Contracts;

namespace MPNotifier.Services {
    public class OfferDetailsService : IOfferDetailsService {
        private readonly IIndex<JobWebsiteTaskProviderType, IOffersService> iindex;

        private readonly IOffersService pracujPlOffersService;

        private readonly IRepository<JobModel> repository;

        private readonly IOffersService trojmiastPlOffersService;

        public OfferDetailsService(IIndex<JobWebsiteTaskProviderType, IOffersService> index, IRepository<JobModel> repository) {
            this.iindex = index;
            this.repository = repository;
            this.pracujPlOffersService = this.iindex[JobWebsiteTaskProviderType.PracujPl];
            this.trojmiastPlOffersService = this.iindex[JobWebsiteTaskProviderType.TrojmiastoPl];
        }

        public OfferDetailsViewModel GetOfferDetails(Guid offerId) {
            var websiteType = this.repository.Filter(x => x.Id == offerId).First().WebsiteType;

            var details = websiteType == JobWebsiteTaskProviderType.PracujPl
                ? this.pracujPlOffersService.GetOfferDetails(offerId)
                : this.trojmiastPlOffersService.GetOfferDetails(offerId);

            return this.ConvertToOfferDetailsViewModel(details);
        }

        private OfferDetailsViewModel ConvertToOfferDetailsViewModel(JobOfferDetailsModel details) {
            return new OfferDetailsViewModel {
                Id = details.Id,
                Company = details.Company,
                Title = details.Title,
                Logo = details.Logo,
                Description = $"{details.CompanyDescription}{Environment.NewLine}{details.OfferDescription}"
            };
        }
    }
}