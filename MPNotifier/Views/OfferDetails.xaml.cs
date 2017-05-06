﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MPNotifier.Core;
using MPNotifier.Models.ViewModels;
using MPNotifier.Services.Contracts;

namespace MPNotifier.Views {
    public sealed partial class OfferDetails : Page {
        public OfferDetailsViewModel ViewModel { get; set; }

        public OfferDetails() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (e.Parameter != null) {
                Guid offerId;
                offerId = (Guid) e.Parameter;
                var offerDetails = IoC.Resolve<IApplicationResultsService>().GetOfferDetails(offerId);
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e) {
            this.SplitView.IsPaneOpen = !this.SplitView.IsPaneOpen;
        }
    }
}