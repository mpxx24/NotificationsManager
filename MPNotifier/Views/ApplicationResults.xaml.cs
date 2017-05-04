using System.Collections.ObjectModel;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JobOffersProvider.Common.Models;
using JobOffersProvider.Core;
using MPNotifier.Services.Contracts;

namespace MPNotifier.Views {
    public sealed partial class ApplicationResults : Page {
        private static Timer timer;

        private readonly IApplicationService applicationService;

        private readonly INotificationsLoader notificationsLoader;

        private readonly IRepository<JobModel> repository;

        private ObservableCollection<JobModel> offers;

        public ApplicationResults() {
            this.InitializeComponent();
        }

        public ApplicationResults(IRepository<JobModel> repository, IApplicationService applicationService, INotificationsLoader notificationsLoader) {
            this.repository = repository;
            this.applicationService = applicationService;
            this.notificationsLoader = notificationsLoader;
            this.InitializeControls();
        }

        private void StartApplicationsLoop() {
            this.PrepareApplicationData();
            timer = new Timer(x => this.ShowNotifications(), null, 1000 * 1, Timeout.Infinite);
        }

        private void PrepareApplicationData() {
            this.applicationService.PrepareApplicationData();
        }

        private void ShowNotifications() {
            this.notificationsLoader.ShowToastNotification(5);

            timer.Change(1000 * 60 * 30, Timeout.Infinite);
        }

        private void InitializeControls() {
            var allOffers = this.repository.GetAll();
            this.offers = new ObservableCollection<JobModel>(allOffers);
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
    }
}