using JobOffersProvider.Common;
using MPNotifier.Helpers;

namespace MPNotifier.Models {
    public class OfferTypeOptionsModel {
        private OfferType offerType { get; }

        public string DisplayValue {
            get {
                switch (this.offerType) {
                    case OfferType.Job:
                        return StringLocalizationsHelper.Jobs;
                    case OfferType.Appartment:
                        return StringLocalizationsHelper.Appartments;
                    default:
                        return string.Empty;
                }
            }
        }

        public OfferTypeOptionsModel(OfferType offerType) {
            this.offerType = offerType;
        }
    }
}