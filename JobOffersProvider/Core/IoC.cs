using Autofac;

namespace JobOffersProvider.Core {
    public static class IoC {
        private static IContainer container;

        private static ContainerBuilder builder;

        public static T Resolve<T>() {
            return container.Resolve<T>();
        }

        public static void Initialize(Module[] modules) {
            builder = new ContainerBuilder();

            if (builder != null) {
                foreach (var module in modules) {
                    builder.RegisterModule(module);
                }
            } else {
                return;
            }

            container = builder.Build();
        }
    }
}