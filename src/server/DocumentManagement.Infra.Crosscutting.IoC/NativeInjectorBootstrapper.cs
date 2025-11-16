using DocumentManagement.Core.Interfaces.Repositories;
using DocumentManagement.Core.Interfaces.Services;
using DocumentManagement.Core.Interfaces.Storage;
using DocumentManagement.Core.Services;
using DocumentManagement.Infra.Data;
using DocumentManagement.Infra.Data.Context;
using DocumentManagement.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentManagement.Infra.Crosscutting.IoC;

public static class NativeInjectorBootstrapper
{
    public static void RegisterServices(this IServiceCollection services)
    {
        //Core - Services
        services.AddScoped<IAuditService, AuditService>();

        //Infra - Repositories
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IDocumentShareRepository, DocumentShareRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();

        //Infra - Storage
        services.AddScoped<IFileStorage, FileSystemFileStorage>();

        //Infra - Contexts
        services.AddScoped<AppDbContext>();

        // Other
        services.AddHttpContextAccessor();
    }
}
