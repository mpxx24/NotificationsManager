using System.Collections.Generic;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using JobOffersProvider.Common;
using JobOffersProvider.Common.Models;

namespace MPNotifier {
    public class NotificationsLoader {
        public async void ShowToastNotification() {
            var xml = new XmlDocument();

            var provider = new PracujPlWebsiteProvider();
            var jobOffers = await provider.GetJobOffers();
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