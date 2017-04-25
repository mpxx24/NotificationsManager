using System.Linq;
using Windows.UI.Notifications;
using Autofac.Features.Indexed;
using JobOffersProvider.Common;

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

        public async void ShowToastNotification() {
            var pracujPlJobOffers = await this.pracujPlOffersService.GetOffers();
            var trojmiastoPlJobOffers = await this.trojmiastPlOffersService.GetOffers();

            var pracujPlJobModels = pracujPlJobOffers.ToList();
            var trojmiastoPlJobModels = trojmiastoPlJobOffers.ToList();

            for (var i = 0; i < 3; i++) {
                var xml = Notifications.GetNotificationContent(pracujPlJobModels[i].Company, pracujPlJobModels[i].Title, pracujPlJobModels[i].Logo, pracujPlJobModels[i].OfferAddress);
                var toast = new ToastNotification(xml);

                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }

            for (var i = 0; i < 3; i++) {
                var xml = Notifications.GetNotificationContent(trojmiastoPlJobModels[i].Company, trojmiastoPlJobModels[i].Title, trojmiastoPlJobModels[i].Logo, trojmiastoPlJobModels[i].OfferAddress);
                var toast = new ToastNotification(xml);

                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
        }
    }
}