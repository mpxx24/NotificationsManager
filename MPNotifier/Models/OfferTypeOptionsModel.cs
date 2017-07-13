namespace MPNotifier.Models {
    public class OfferTypeOptionsModel {
        private string offerType { get; }

        public string DisplayValue => this.offerType;

        public OfferTypeOptionsModel(string offerType) {
            this.offerType = offerType;
        }
    }
}