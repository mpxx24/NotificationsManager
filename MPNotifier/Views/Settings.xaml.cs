using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Autofac;
using JobOffersProvider.Common;
using JobOffersProvider.Common.Models;
using JobOffersProvider.Core;
using JobOffersProvider.Sites.PracujPl;
using JobOffersProvider.Sites.TrojmiastoPl;
using MPNotifier.Models;
using MPNotifier.Services;
using MPNotifier.Services.Contracts;

namespace MPNotifier.Views {
    public sealed partial class Settings : Page {
        private static IContainer container { get; set; }

        private ObservableCollection<TimerOptionsModel> availableTimeOptions { get; set; }

        private ObservableCollection<OfferTypeOptionsModel> availableOfferTypeOptions { get; set; }

        public Settings() {
            this.InitializeComponent();
            this.RegisterComponents();

            this.InitializeControls();
        }

        private void RegisterComponents() {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<PracujPlWebsiteProvider>().Keyed<IJobWebsiteTask>(JobWebsiteTaskProviderType.PracujPl);
            containerBuilder.RegisterType<PracujPlOffersService>().Keyed<IOffersService>(JobWebsiteTaskProviderType.PracujPl);
            containerBuilder.RegisterType<TrojmiastoPlWebsiteProvider>().Keyed<IJobWebsiteTask>(JobWebsiteTaskProviderType.TrojmiastoPl);
            containerBuilder.RegisterType<TrojmiastoPlOffersService>().Keyed<IOffersService>(JobWebsiteTaskProviderType.TrojmiastoPl);
            containerBuilder.RegisterType<ApplicationService>().As<IApplicationService>();
            containerBuilder.RegisterType<JobModelRepository>().As<IRepository<JobModel>>();
            containerBuilder.RegisterType<NotificationsLoader>().As<INotificationsLoader>();

            containerBuilder.RegisterType<ApplicationResults>();

            container = containerBuilder.Build();
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
                this.Frame.Navigate(typeof(ApplicationResults), true);
            }
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e) {
            this.SplitView.IsPaneOpen = !this.SplitView.IsPaneOpen;
        }
    }
}