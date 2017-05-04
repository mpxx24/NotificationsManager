using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JobOffersProvider.Common.Models;
using JobOffersProvider.Core;
using MPNotifier.Core;
using MPNotifier.Models.ViewModels;
using MPNotifier.Services.Contracts;

namespace MPNotifier.Views {
    public sealed partial class ApplicationResults : Page {
        public ApplicationResultsViewModel ViewModel { get; set; }
        private static Timer timer;

        private ObservableCollection<JobModel> offers;
        
        public ApplicationResults() {
            this.InitializeComponent();
            this.PrepareApplicationData();
            this.InitializeControls();
            this.ViewModel = new ApplicationResultsViewModel();
        }

        private void StartApplicationsLoop() {
            timer = new Timer(x => this.ShowNotifications(), null, 1000 * 1, Timeout.Infinite);
        }

        private void PrepareApplicationData() {
            IoC.Resolve<IApplicationService>().PrepareApplicationData();
        }

        private void ShowNotifications() {
            IoC.Resolve<INotificationsLoader>().ShowToastNotification(5);

            timer.Change(1000 * 60 * 30, Timeout.Infinite);
        }

        private void InitializeControls() {
            var allOffers = IoC.Resolve<IRepository<JobModel>>().GetAll();
            this.offers = new ObservableCollection<JobModel>(allOffers);
            this.ResultsListView.ItemsSource = this.offers;
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