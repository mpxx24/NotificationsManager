using System.Collections.Generic;
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
    }
}