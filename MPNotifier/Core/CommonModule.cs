using Autofac;
using JobOffersProvider.Common;
using JobOffersProvider.Common.Models;
using JobOffersProvider.Core;
using JobOffersProvider.Sites.PracujPl;
using JobOffersProvider.Sites.TrojmiastoPl;
using MPNotifier.Services;
using MPNotifier.Services.Contracts;

namespace MPNotifier.Core {
    public class CommonModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<PracujPlWebsiteProvider>().Keyed<IJobWebsiteTask>(JobWebsiteTaskProviderType.PracujPl);
            builder.RegisterType<PracujPlOffersService>().Keyed<IOffersService>(JobWebsiteTaskProviderType.PracujPl);
            builder.RegisterType<TrojmiastoPlWebsiteProvider>().Keyed<IJobWebsiteTask>(JobWebsiteTaskProviderType.TrojmiastoPl);
            builder.RegisterType<TrojmiastoPlOffersService>().Keyed<IOffersService>(JobWebsiteTaskProviderType.TrojmiastoPl);
            builder.RegisterType<ApplicationService>().As<IApplicationService>();
            builder.RegisterType<JobModelRepository>().As<IRepository<JobModel>>();
            builder.RegisterType<NotificationsLoader>().As<INotificationsLoader>();
            builder.RegisterType<ApplicationResultsService>().As<IApplicationResultsService>();
        }
    }
}