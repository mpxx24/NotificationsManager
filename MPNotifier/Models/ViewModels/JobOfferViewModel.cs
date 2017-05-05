using System.Collections.Generic;
using JobOffersProvider.Common.Models;

namespace MPNotifier.Models.ViewModels {
    public class JobOfferViewModel {
        public string Title { get; set; }

        public string Logo { get; set; }

        public static IEnumerable<JobOfferViewModel> ConvertToJobOfferViewModels(IEnumerable<JobModel> offers) {
            var result = new List<JobOfferViewModel>();

            foreach (var jobModel in offers) {
                result.Add(new JobOfferViewModel {
                    Title = $" {jobModel.Company} - {jobModel.Title}",
                    Logo = jobModel.Logo
                });
            }

            return result;
        }
    }
}