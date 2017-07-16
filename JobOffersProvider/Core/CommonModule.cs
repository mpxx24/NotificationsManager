using Autofac;
using JobOffersProvider.Common;
using JobOffersProvider.Common.Models;
using JobOffersProvider.Sites.PracujPl;
using JobOffersProvider.Sites.TrojmiastoPl;

namespace JobOffersProvider.Core {
    public class CommonModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<PracujPlWebsiteProvider>().Keyed<IJobWebsiteTask>(WebsiteType.PracujPl);
            builder.RegisterType<PracujPlOffersService>().Keyed<IOffersService>(WebsiteType.PracujPl);
            builder.RegisterType<TrojmiastoPlWebsiteProvider>().Keyed<IJobWebsiteTask>(WebsiteType.TrojmiastoPl);
            builder.RegisterType<TrojmiastoPlOffersService>().Keyed<IOffersService>(WebsiteType.TrojmiastoPl);
            builder.RegisterType<JobModelRepository>().As<IRepository<JobModel>>();
        }
    }
}