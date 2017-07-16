using JobOffersProvider.Common;

namespace MPNotifier.Models {
    public class ApplicationSettingsModel {
        public string SearchText { get; set; }

        public OfferType OfferType { get; set; }

        public WebsiteType Website { get; set; }

        public TimeIntervalType IntervalType { get; set; }

        public bool? ShouldSendEmail { get; set; }
    }
}