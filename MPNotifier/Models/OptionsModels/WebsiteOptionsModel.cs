using JobOffersProvider.Common;
using MPNotifier.Helpers;

namespace MPNotifier.Models {
    public class WebsiteOptionsModel {
        private WebsiteType websiteType { get; }

        public string DisplayValue {
            get {
                switch (this.websiteType) {
                    case WebsiteType.PracujPl:
                        return StringLocalizationsHelper.PracujPl;
                    case WebsiteType.TrojmiastoPl:
                        return StringLocalizationsHelper.TrojmiastoPL;
                    default:
                        return string.Empty;
                }
            }
        }

        public WebsiteOptionsModel(WebsiteType websiteType) {
            this.websiteType = websiteType;
        }
    }
}