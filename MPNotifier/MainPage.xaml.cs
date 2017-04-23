using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Autofac;
using JobOffersProvider.Common;

namespace MPNotifier {
    public sealed partial class MainPage : Page {
        private static IContainer container { get; set; }

        public MainPage() {
            this.InitializeComponent();
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<PracujPlWebsiteProvider>().As<IJobWebsiteTask>();
            containerBuilder.RegisterType<NotificationsLoader>().As<INotificationsLoader>();

            container = containerBuilder.Build();
        }

        private void Button_OnClick(object sender, RoutedEventArgs e) {
            var notificationsLoader = container.Resolve<INotificationsLoader>();
            notificationsLoader.ShowToastNotification();
        }
    }
}