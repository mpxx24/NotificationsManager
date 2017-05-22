using System;
using System.Collections.ObjectModel;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using JobOffersProvider.Core;
using MPNotifier.Helpers;
using MPNotifier.Models;
using MPNotifier.Services.Contracts;

namespace MPNotifier.Views {
    public sealed partial class Settings : Page {
        private ObservableCollection<TimerOptionsModel> availableTimeOptions { get; set; }

        private ObservableCollection<OfferTypeOptionsModel> availableOfferTypeOptions { get; set; }

        public Settings() {
            this.InitializeComponent();
            this.SetWindowSize();
            this.InitializeControls();
            this.PrepareApplicationData();
        }

        private void PrepareApplicationData() {
            try {
                IoC.Resolve<IApplicationService>().PrepareApplicationData();
            } catch (Exception) {
                //TODO: logger
            }
        }

        private void SetWindowSize() {
            ApplicationView.PreferredLaunchViewSize = new Size(1000, 600);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        private void InitializeControls() {
            this.availableTimeOptions = new ObservableCollection<TimerOptionsModel> {
                new TimerOptionsModel(30),
                new TimerOptionsModel(60),
                new TimerOptionsModel(90)
            };

            this.availableOfferTypeOptions = new ObservableCollection<OfferTypeOptionsModel> {
                new OfferTypeOptionsModel("Jobs"),
                new OfferTypeOptionsModel("Appartments")
            };
        }

        private void StartApplicationButton_OnClick(object sender, RoutedEventArgs e) {
            var selectedOfferTypeItem = this.OfferTypeCombo.SelectedItem;
            var selectedTimerItem = this.TimerCombo.SelectedItem;
            var offerTypeOptionsModel = (OfferTypeOptionsModel) selectedOfferTypeItem;
            var timerOptionsModel = (TimerOptionsModel) selectedTimerItem;

            this.TimerComboValidation.Visibility = timerOptionsModel == null ? Visibility.Visible : Visibility.Collapsed;
            this.OfferComboValidation.Visibility = offerTypeOptionsModel == null ? Visibility.Visible : Visibility.Collapsed;

            if (timerOptionsModel != null && offerTypeOptionsModel != null) {
                NavigationHelper.Navigate(typeof(ApplicationResults), true);
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e) {
            this.SplitView.IsPaneOpen = !this.SplitView.IsPaneOpen;
        }
    }
}