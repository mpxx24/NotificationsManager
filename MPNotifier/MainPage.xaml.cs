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

namespace MPNotifier {
    public sealed partial class MainPage : Page {
        private static IContainer container { get; set; }

        private static Timer timer;

        private ObservableCollection<string> availableTimeOptions { get; set; }


        public MainPage() {
            this.InitializeComponent();
            this.RegisterComponents();

            this.SetApplicationSize();
            this.InitializeControls();

            this.StartApplicationsLoop();

        }
        private void StartApplicationsLoop() {
            timer = new Timer(x => ShowNotifications(), null, 1000 * 60, Timeout.Infinite);
        }

        private void InitializeControls() {
            this.availableTimeOptions = new ObservableCollection<string> {"10min", "30min", "1h"};
        }

        private void SetApplicationSize() {
            ApplicationView.PreferredLaunchViewSize = new Size(300, 200);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
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

        private static void ShowNotifications() {
            using (var scope = container.BeginLifetimeScope()) {
                var notificationsLoader = scope.Resolve<INotificationsLoader>();
                notificationsLoader.ShowToastNotification();
            }
            timer.Change(1000 * 60, Timeout.Infinite);
        }
        private void SaveChangesButton_OnClick(object sender, RoutedEventArgs e) {
            
        }
    }
}