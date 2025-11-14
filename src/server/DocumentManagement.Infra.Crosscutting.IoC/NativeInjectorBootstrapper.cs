using DocumentManagement.Core.Interfaces.Repositories;
using DocumentManagement.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentManagement.Infra.Crosscutting.IoC;

public static class NativeInjectorBootstrapper
{
    public static void RegisterServices(this IServiceCollection services)
    {

        // Repositories
        services.AddScoped<IAuthRepository, AuthRepository>();
    }
}
