using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;
using JobOffersProvider.Common;
using JobOffersProvider.Common.Models;

namespace MPNotifier {
    public class NotificationsLoader : INotificationsLoader {
        private readonly IJobWebsiteTask jobWebsiteTask;

        public NotificationsLoader(IJobWebsiteTask jobWebsiteTask) {
            this.jobWebsiteTask = jobWebsiteTask;
        }

        public async void ShowToastNotification() {
            var jobOffers = await this.jobWebsiteTask.GetJobOffers();
            var jobModels = jobOffers as IList<JobModel> ?? jobOffers.ToList();

            for (var i = 0; i < 10; i++) {
                var xml = Notifications.GetNotificationContent(jobModels[i].Company, jobModels[i].Title, jobModels[i].Logo, jobModels[i].OfferAddress);
                var toast = new ToastNotification(xml);

                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
        }
    }
}