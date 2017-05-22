using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JobOffersProvider.Core;
using MPNotifier.Helpers;
using MPNotifier.Models.ViewModels;
using MPNotifier.Services.Contracts;

namespace MPNotifier.Views {
    public sealed partial class OfferDetails : Page {
        private Guid offerId { get; set; }

        public OfferDetailsViewModel ViewModel { get; set; }

        public OfferDetails() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (e.Parameter != null) {
                this.offerId = (Guid) e.Parameter;
                this.ViewModel = IoC.Resolve<IOfferDetailsService>().GetOfferDetails(this.offerId);
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e) {
            this.SplitView.IsPaneOpen = !this.SplitView.IsPaneOpen;
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e) {
            NavigationHelper.Navigate(typeof(ApplicationResults), false);
        }
    }
}