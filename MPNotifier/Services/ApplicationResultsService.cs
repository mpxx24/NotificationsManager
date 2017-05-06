using System;
using System.Collections.Generic;
using System.Linq;
using JobOffersProvider.Common.Models;
using JobOffersProvider.Core;
using MPNotifier.Models.ViewModels;
using MPNotifier.Services.Contracts;

namespace MPNotifier.Services {
    public class ApplicationResultsService : IApplicationResultsService {
        private readonly IRepository<JobModel> repository;
        public ApplicationResultsService(IRepository<JobModel> repository) {
            this.repository = repository;
        }

        public IEnumerable<JobOfferViewModel> GetAllJobOfferViewModels() {
            var offers = this.repository.GetAll();

            return JobOfferViewModel.ConvertToJobOfferViewModels(offers);
        }

        public JobOfferDetailsViewModel GetOfferDetails(Guid offerId) {
            var jobModel = this.repository.Filter(x => x.Id == offerId).First();
            return this.ConvertToJofOfferDetailsViewModel(jobModel);
        }

        private JobOfferDetailsViewModel ConvertToJofOfferDetailsViewModel(JobModel jobModel) {
            return new JobOfferDetailsViewModel {
                Id = jobModel.Id,
                Company = jobModel.Company,
                Title = jobModel.Title,
                Logo = jobModel.Logo,
                Description = jobModel.Description
            };
        }
    }
}