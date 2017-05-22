using Autofac;
using MPNotifier.Services;
using MPNotifier.Services.Contracts;

namespace MPNotifier.Core.Modules {
    public class ApplicationModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<ApplicationService>().As<IApplicationService>();
            builder.RegisterType<NotificationsLoader>().As<INotificationsLoader>();
            builder.RegisterType<ApplicationResultsService>().As<IApplicationResultsService>();
            builder.RegisterType<OfferDetailsService>().As<IOfferDetailsService>();
        }
    }
}