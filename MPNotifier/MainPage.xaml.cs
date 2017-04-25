using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Autofac;
using JobOffersProvider.Common;
using JobOffersProvider.Sites.PracujPl;
using JobOffersProvider.Sites.TrojmiastoPl;

namespace MPNotifier {
    public sealed partial class MainPage : Page {
        private static IContainer container { get; set; }

        public MainPage() {
            this.InitializeComponent();
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<PracujPlWebsiteProvider>().Keyed<IJobWebsiteTask>(JobWebsiteTaskProviderType.PracujPl);
            containerBuilder.RegisterType<PracujPlOffersService>().Keyed<IOffersService>(JobWebsiteTaskProviderType.PracujPl);
            containerBuilder.RegisterType<TrojmiastoPlWebsiteProvider>().Keyed<IJobWebsiteTask>(JobWebsiteTaskProviderType.TrojmiastoPl);
            containerBuilder.RegisterType<TrojmiastoPlOffersService>().Keyed<IOffersService>(JobWebsiteTaskProviderType.TrojmiastoPl);
            containerBuilder.RegisterType<NotificationsLoader>().As<INotificationsLoader>();

            container = containerBuilder.Build();
        }

        private void Button_OnClick(object sender, RoutedEventArgs e) {
            using (var scope = container.BeginLifetimeScope()) {
                var notificationsLoader = scope.Resolve<INotificationsLoader>();
                notificationsLoader.ShowToastNotification();
            }
        }
    }
}