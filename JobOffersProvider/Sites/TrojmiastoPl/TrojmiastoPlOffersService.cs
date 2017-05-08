using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using JobOffersProvider.Common;
using JobOffersProvider.Common.Models;
using JobOffersProvider.Core;

namespace JobOffersProvider.Sites.TrojmiastoPl {
    public class TrojmiastoPlOffersService : IOffersService {
        private readonly IIndex<JobWebsiteTaskProviderType, IJobWebsiteTask> iJobWebsiteTaskIndex;

        private readonly IJobWebsiteTask jobWebsiteTask;

        private readonly IRepository<JobModel> repository;

        public TrojmiastoPlOffersService(IIndex<JobWebsiteTaskProviderType, IJobWebsiteTask> index, IRepository<JobModel> repository) {
            this.iJobWebsiteTaskIndex = index;
            this.repository = repository;
            this.jobWebsiteTask = this.iJobWebsiteTaskIndex[JobWebsiteTaskProviderType.TrojmiastoPl];
        }

        public IEnumerable<JobModel> GetOffers() {
            var jobOffers = this.jobWebsiteTask.GetJobOffers();
            return jobOffers.Result.ToList();
        }
        public JobOfferDetailsModel GetOfferDetails(Guid offerId) {
            var jobOffer = this.repository.Filter(x => x.Id == offerId).First();
            var jobModel = this.jobWebsiteTask.GetJobOfferDetails(jobOffer.OfferAddress);

            return new JobOfferDetailsModel {
                Id = jobOffer.Id,
                Company = jobOffer.Company,
                Title = jobOffer.Title,
                Logo = jobOffer.Logo,
                OfferAddress = jobOffer.OfferAddress,
                Added = jobOffer.Added,
                Cities = jobOffer.Cities,
                CompanyDescription = jobModel.Result.CompanyDescription,
                OfferDescription = jobModel.Result.OfferDescription
            };
        }
    }
}