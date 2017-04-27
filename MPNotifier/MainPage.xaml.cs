using System.Collections.ObjectModel;
using System.Threading;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Autofac;
using JobOffersProvider.Common;
using JobOffersProvider.Sites.PracujPl;
using JobOffersProvider.Sites.TrojmiastoPl;
using MPNotifier.Models;

namespace MPNotifier {
    public sealed partial class MainPage : Page {
        private static IContainer container { get; set; }

        private static Timer timer;

        private static int timerInterval { get; set; }

        private bool isApplicationStarted { get; set; }

        private ObservableCollection<TimerOptionsModel> availableTimeOptions { get; set; }
        private ObservableCollection<OfferTypeOptionsModel> availableOfferTypeOptions { get; set; }

        public MainPage() {
            this.InitializeComponent();
            this.RegisterComponents();

            this.SetApplicationSize();
            this.InitializeControls();
        }

        private void RegisterComponents() {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<PracujPlWebsiteProvider>().Keyed<IJobWebsiteTask>(JobWebsiteTaskProviderType.PracujPl);
            containerBuilder.RegisterType<PracujPlOffersService>().Keyed<IOffersService>(JobWebsiteTaskProviderType.PracujPl);
            containerBuilder.RegisterType<TrojmiastoPlWebsiteProvider>().Keyed<IJobWebsiteTask>(JobWebsiteTaskProviderType.TrojmiastoPl);
            containerBuilder.RegisterType<TrojmiastoPlOffersService>().Keyed<IOffersService>(JobWebsiteTaskProviderType.TrojmiastoPl);
            containerBuilder.RegisterType<NotificationsLoader>().As<INotificationsLoader>();

            container = containerBuilder.Build();
        }

        private void StartApplicationsLoop() {
            timer = new Timer(x => ShowNotifications(), null, 1000 * 10, Timeout.Infinite);
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

        private void SetApplicationSize() {
            ApplicationView.PreferredLaunchViewSize = new Size(800, 200);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        private static void ShowNotifications() {
            using (var scope = container.BeginLifetimeScope()) {
                var notificationsLoader = scope.Resolve<INotificationsLoader>();
                notificationsLoader.ShowToastNotification();
            }
            timer.Change(1000 * 60 * timerInterval, Timeout.Infinite);
        }

        private void SaveChangesButton_OnClick(object sender, RoutedEventArgs e) {
            var selectedOfferTypeItem = this.OfferTypeCombo.SelectedItem;
            var selectedTimerItem = this.TimerCombo.SelectedItem;
            var offerTypeOptionsModel = (OfferTypeOptionsModel)selectedOfferTypeItem;
            var timerOptionsModel = (TimerOptionsModel)selectedTimerItem;

            if (timerOptionsModel != null && offerTypeOptionsModel != null) {
                timerInterval = timerOptionsModel.Minutes;
                this.StartApplicationsLoop();
                this.isApplicationStarted = true;

                this.SetItemsVisibilityAndContent(true);
            }
            else {
                this.SetItemsVisibilityAndContent(false);
            }
        }

        private void SetItemsVisibilityAndContent(bool valid) {
            if (valid) {
                this.TimerComboValidation.Visibility = Visibility.Collapsed;
                this.OfferComboValidation.Visibility = Visibility.Collapsed;
                this.SaveChangesButton.Content = "Apply changes";
            } else {
                this.TimerComboValidation.Visibility = Visibility.Visible;
                this.OfferComboValidation.Visibility = Visibility.Visible;
            }
        }
    }
}