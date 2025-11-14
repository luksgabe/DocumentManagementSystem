using Microsoft.AspNetCore.RateLimiting;

namespace DocumentManagement.Api.Configurations;

public static class RateLimiterConfig
{
    public static void AddRateLimiterConfiguration(this IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentException("services");
        }

        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("default", config =>
            {
                config.PermitLimit = 100;
                config.Window = TimeSpan.FromMinutes(1);
                config.QueueLimit = 0;
            });
        });
    }
}
