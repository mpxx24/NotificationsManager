namespace MPNotifier.Models {
    public class WebsiteOptionsModel {
        private string website { get; }

        public string DisplayValue => this.website;

        public WebsiteOptionsModel(string site) {
            this.website = site;
        }
    }
}