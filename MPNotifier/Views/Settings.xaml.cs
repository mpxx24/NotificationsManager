using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using JobOffersProvider.Common;
using MPNotifier.Helpers;
using MPNotifier.Models;

namespace MPNotifier.Views {
    public sealed partial class Settings : Page {
        private ObservableCollection<TimerOptionsModel> availableTimeOptions { get; set; }

        private ObservableCollection<OfferTypeOptionsModel> availableOfferTypeOptions { get; set; }

        private ObservableCollection<WebsiteOptionsModel> websiteOptions { get; set; }

        public Settings() {
            this.InitializeComponent();
            this.InitializeControls();
        }

        private void InitializeControls() {
            this.availableTimeOptions = new ObservableCollection<TimerOptionsModel> {
                new TimerOptionsModel(TimeIntervalType.HalfAnHour),
                new TimerOptionsModel(TimeIntervalType.Hour),
                new TimerOptionsModel(TimeIntervalType.HourAndAHalf)
            };

            this.availableOfferTypeOptions = new ObservableCollection<OfferTypeOptionsModel> {
                new OfferTypeOptionsModel(OfferType.Job),
                new OfferTypeOptionsModel(OfferType.Appartment)
            };

            this.websiteOptions = new ObservableCollection<WebsiteOptionsModel> {
                new WebsiteOptionsModel(WebsiteType.PracujPl),
                new WebsiteOptionsModel(WebsiteType.TrojmiastoPl)
            };
        }

        private void StartApplicationButton_OnClick(object sender, RoutedEventArgs e) {
            var settings = this.RetrieveSettings();

            //this.TimerComboValidation.Visibility = settings.IntervalType == TimeIntervalType.Undefined ? Visibility.Visible : Visibility.Collapsed;
            //this.OfferComboValidation.Visibility = settings.OfferType == OfferType.Undefined ? Visibility.Visible : Visibility.Collapsed;

            if (settings.IntervalType != TimeIntervalType.Undefined && settings.OfferType != OfferType.Undefined) {
                NavigationHelper.Navigate(new NavigationModel {ViewType = typeof(ApplicationResults), Parameter = settings});
            }
        }

        private ApplicationSettingsModel RetrieveSettings() {
            var offerTypeOptionsModel = (OfferTypeOptionsModel) this.OfferTypeCombo.SelectedItem;
            var websiteOptionsModel = (WebsiteOptionsModel) this.SitesCombo.SelectedItem;
            var timerOptionsModel = (TimerOptionsModel) this.TimerCombo.SelectedItem;

            return new ApplicationSettingsModel {
                ShouldSendEmail = this.SendEmailCheckbox.IsChecked,
                SearchText = this.SearchTextBox.Text,
                OfferType = this.GetofferType(offerTypeOptionsModel),
                Website = this.GetWebsiteType(websiteOptionsModel),
                IntervalType = this.GetIntervalType(timerOptionsModel)
            };
        }

        private TimeIntervalType GetIntervalType(TimerOptionsModel timerOptionsModel) {
            return timerOptionsModel?.IntervalType ?? TimeIntervalType.Undefined;
        }

        private WebsiteType GetWebsiteType(WebsiteOptionsModel websiteOptionsModel) {
            if (websiteOptionsModel == null) {
                return WebsiteType.Undefined;
            }

            return websiteOptionsModel.DisplayValue == StringLocalizationsHelper.PracujPl
                ? WebsiteType.PracujPl
                : WebsiteType.TrojmiastoPl;
        }

        private OfferType GetofferType(OfferTypeOptionsModel offerTypeOptionsModel) {
            if (offerTypeOptionsModel == null) {
                return OfferType.Undefined;
            }

            return offerTypeOptionsModel.DisplayValue == StringLocalizationsHelper.Jobs
                ? OfferType.Job
                : OfferType.Appartment;
        }
    }
}