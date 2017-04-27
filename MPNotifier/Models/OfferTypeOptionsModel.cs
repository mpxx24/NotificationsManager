namespace MPNotifier.Models {
    public class OfferTypeOptionsModel {
        private string OfferType { get; }

        public string DisplayValue => this.OfferType;

        public OfferTypeOptionsModel(string offerType) {
            this.OfferType = offerType;
        }
    }
}