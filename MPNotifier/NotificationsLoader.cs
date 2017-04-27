using System.Collections.Generic;
using System.Linq;
using Windows.UI.Notifications;
using Autofac.Features.Indexed;
using JobOffersProvider.Common;
using JobOffersProvider.Common.Models;

namespace MPNotifier {
    public class NotificationsLoader : INotificationsLoader {
        private readonly IIndex<JobWebsiteTaskProviderType, IOffersService> iindex;

        private readonly IOffersService pracujPlOffersService;

        private readonly IOffersService trojmiastPlOffersService;

        public NotificationsLoader(IIndex<JobWebsiteTaskProviderType, IOffersService> index) {
            this.iindex = index;
            this.pracujPlOffersService = this.iindex[JobWebsiteTaskProviderType.PracujPl];
            this.trojmiastPlOffersService = this.iindex[JobWebsiteTaskProviderType.TrojmiastoPl];
        }

        public void ShowToastNotification() {
            var jobOffers = this.GetJobOffers();

            for (var i = 0; i < 5; i++) {
                var xml = Notifications.GetNotificationContent(jobOffers[i].Company, jobOffers[i].Title, jobOffers[i].Logo, jobOffers[i].OfferAddress);
                var toast = new ToastNotification(xml);

                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
        }

        private List<JobModel> GetJobOffers() {
            var jobOffers = new List<JobModel>();

            var pracujPlJobOffers = this.pracujPlOffersService.GetOffers();
            var trojmiastoPlJobOffers = this.trojmiastPlOffersService.GetOffers();
            jobOffers.AddRange(pracujPlJobOffers);
            jobOffers.AddRange(trojmiastoPlJobOffers);

            var preparedOffers = this.PrepareJobOffers(jobOffers);

            return preparedOffers.ToList();
        }

        private IEnumerable<JobModel> PrepareJobOffers(IEnumerable<JobModel> jobModels) {
            var jobList = jobModels.ToList();

            var distinctOffers = jobList.GroupBy(x => new {x.Company, x.Title}).Select(y => y.First()).ToList();

            var sortedfOffers = distinctOffers.OrderByDescending(x => x.Added).ToList();

            return sortedfOffers;
        }
    }
}