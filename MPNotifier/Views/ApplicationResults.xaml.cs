using System;
using System.Collections.ObjectModel;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MPNotifier.Core;
using MPNotifier.Models.ViewModels;
using MPNotifier.Services.Contracts;

namespace MPNotifier.Views {
    public sealed partial class ApplicationResults : Page {
        private static Timer timer;

        public ApplicationResultsViewModel ViewModel { get; set; }

        public ApplicationResults() {
            this.InitializeComponent();
            this.ViewModel = new ApplicationResultsViewModel();

            this.InitializeApplication();
        }
        private void InitializeApplication() {
            this.PrepareApplicationData();
            this.InitializeControls();
        }

        private void StartApplicationsLoop() {
            timer = new Timer(x => this.ShowNotifications(), null, 1000 * 1, Timeout.Infinite);
        }

        private void PrepareApplicationData() {
            try {
                IoC.Resolve<IApplicationService>().PrepareApplicationData();
            } catch (Exception ex) {
                //TODO: logger
            }
        }

        private void ShowNotifications() {
            IoC.Resolve<INotificationsLoader>().ShowToastNotification(5);

            timer.Change(1000 * 60 * 30, Timeout.Infinite);
        }

        private void InitializeControls() {
            var allOffers = IoC.Resolve<IApplicationResultsService>().GetAllJobOfferViewModels();
            this.ViewModel.Offers = new ObservableCollection<JobOfferViewModel>(allOffers);
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e) {
            this.SplitView.IsPaneOpen = !this.SplitView.IsPaneOpen;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            var isAfterApplicationStart = e.Parameter != null && (bool) e.Parameter;
            if (isAfterApplicationStart) {
                this.StartApplicationsLoop();
            }
        }

        private void ResultsListView_OnItemClick(object sender, ItemClickEventArgs e) {
            var clickedItem = (JobOfferViewModel) e.ClickedItem;
            this.ShowOfferDetails(clickedItem.Id);
        }

        private void ShowOfferDetails(Guid clickedItemId) {
            this.Frame.Navigate(typeof(OfferDetails), clickedItemId);
        }
    }
}