namespace JobOffersProvider.Common.Models {
    public class SearchSettingsModel {
        public string Text { get; set; }

        public OfferType OfferType { get; set; }

        public WebsiteType Website { get; set; }
    }
}