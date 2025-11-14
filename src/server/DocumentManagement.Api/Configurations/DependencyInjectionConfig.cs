using DocumentManagement.Infra.Crosscutting.IoC;

namespace DocumentManagement.Api.Configurations;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));


        NativeInjectorBootstrapper.RegisterServices(services);
    }
}
