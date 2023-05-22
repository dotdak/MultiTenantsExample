using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantsExample.Core.Settings;
using MultiTenantsExample.Infrastructure.Persistence;

namespace MultiTenantsExample.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAndMigrateTenantDatabase(this IServiceCollection services, IConfiguration config)
    {
        var options = services.GetOptions<TenantSettings>(nameof(TenantSettings));
        var defaultConnectionString = options.Defaults?.ConnectionString;
        var defaultDbProvider = options.Defaults?.DBProvider;
        if (defaultDbProvider?.ToLower() == "mssql")
        {
            services.AddDbContext<ApplicationDbContext>(m => m.UseSqlServer(defaultConnectionString));
        }
        var tenants = options.Tenants;
        foreach (var tenant in tenants)
        {
            var connectionString = string.IsNullOrEmpty(tenant.ConnectionString) ? defaultConnectionString : tenant.ConnectionString;
            using var scope = services.BuildServiceProvider().CreateScope();
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.SetConnectionString(connectionString);
                if (dbContext.Database.GetMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
            }
        }
        return services;
    }

    public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
    {
        using var servicesProvider = services.BuildServiceProvider();
        {
            var configuration = servicesProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);
            return options;
        }
    }
}
