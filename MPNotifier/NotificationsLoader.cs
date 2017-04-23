using System.Collections.Generic;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using JobOffersProvider.Common;
using JobOffersProvider.Common.Models;

namespace MPNotifier {
    public class NotificationsLoader : INotificationsLoader {
        private IJobWebsiteTask jobWebsiteTask;
        public NotificationsLoader(IJobWebsiteTask jobWebsiteTask) {
            this.jobWebsiteTask = jobWebsiteTask;
        }

        public async void ShowToastNotification() {
            var xml = new XmlDocument();
          
            var jobOffers = await this.jobWebsiteTask.GetJobOffers();
            var jobModels = jobOffers as IList<JobModel> ?? jobOffers.ToList();

            for (var i = 0; i < 3; i++) {
                xml.LoadXml(Notifications.GetNotification(PrepareStringToPassToXml(jobModels[i].Company), PrepareStringToPassToXml(jobModels[i].Title), jobModels[i].Logo, jobModels[i].OfferAddress));
                var toast = new ToastNotification(xml);

                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
        }

        private static string PrepareStringToPassToXml(string data) {
            return data.Replace("&", "and");
        }
    }
}