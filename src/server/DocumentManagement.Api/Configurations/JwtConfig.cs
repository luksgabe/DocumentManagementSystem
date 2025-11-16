using DocumentManagement.Infra.Data.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DocumentManagement.Api.Configurations;

public static class JwtConfig
{
    public static IServiceCollection AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null)
        {
            throw new ArgumentException("services");
        }

        if (configuration == null)
        {
            throw new ArgumentException("configuration");
        }

        IConfigurationSection section = configuration.GetSection("AppSettings");
        services.Configure<AppJwtSettings>(section);
        AppJwtSettings appSettings = section.Get<AppJwtSettings>()!;
        byte[] key = Encoding.ASCII.GetBytes(appSettings.SecretKey);

        services.AddAuthentication("Bearer")
            .AddJwtBearer(delegate (JwtBearerOptions x)
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidAudience = appSettings.Audience,
                    ValidIssuer = appSettings.Issuer
                };
            });
        return services;
    }
}
