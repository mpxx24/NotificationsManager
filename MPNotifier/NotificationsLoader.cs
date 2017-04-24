using System.Collections.Generic;
using System.Linq;
using Windows.UI.Notifications;
using Autofac.Features.Indexed;
using JobOffersProvider.Common;
using JobOffersProvider.Common.Models;

namespace MPNotifier {
    public class NotificationsLoader : INotificationsLoader {
        private readonly IIndex<JobWebsiteTaskProviderType, IJobWebsiteTask> iindex;

        private readonly IJobWebsiteTask jobWebsiteTask;


        public NotificationsLoader(IIndex<JobWebsiteTaskProviderType, IJobWebsiteTask> index) {
            this.iindex = index;
            this.jobWebsiteTask = this.iindex[JobWebsiteTaskProviderType.PracujPl];
        }

        public async void ShowToastNotification() {
            var jobOffers = await this.jobWebsiteTask.GetJobOffers();
            var jobModels = jobOffers as IList<JobModel> ?? jobOffers.ToList();

            for (var i = 0; i < 3; i++) {
                var xml = Notifications.GetNotificationContent(jobModels[i].Company, jobModels[i].Title, jobModels[i].Logo, jobModels[i].OfferAddress);
                var toast = new ToastNotification(xml);

                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
        }
    }
}