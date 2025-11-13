using DocumentManagement.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;


namespace DocumentManagement.Api.Configurations;
public static class DatabaseConfig
{
    public static void AddDatabaseConfiguration(this WebApplicationBuilder builder)
    {
        if (builder.Services == null) throw new ArgumentNullException(nameof(builder.Services));

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    }
}
