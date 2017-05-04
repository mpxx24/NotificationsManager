using System.Collections.Generic;
using System.Linq;
using Windows.UI.Notifications;
using JobOffersProvider.Common.Models;
using JobOffersProvider.Core;

namespace MPNotifier {
    public class NotificationsLoader : INotificationsLoader {
        private readonly IRepository<JobModel> repository;

        public NotificationsLoader(IRepository<JobModel> repository) {
            this.repository = repository;
        }

        public void ShowToastNotification(int numberOfOffers) {
            var allOffers = this.repository.GetAll();
            var jobOffers = this.PrepareJobOffers(allOffers);

            if (numberOfOffers > jobOffers.Count) {
                numberOfOffers = jobOffers.Count;
            }

            for (var i = 0; i < numberOfOffers; i++) {
                var xml = Notifications.GetNotificationContent(jobOffers[i].Company, jobOffers[i].Title, jobOffers[i].Logo, jobOffers[i].OfferAddress);
                var toast = new ToastNotification(xml);

                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
        }

        private List<JobModel> PrepareJobOffers(IEnumerable<JobModel> jobModels) {
            var distinctOffers = jobModels.GroupBy(x => new {x.Company, x.Title}).Select(y => y.First());

            var sortedfOffers = distinctOffers.OrderByDescending(x => x.Added).ToList();

            return sortedfOffers;
        }
    }
}