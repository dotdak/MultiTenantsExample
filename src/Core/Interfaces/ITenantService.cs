using MultiTenantsExample.Core.Settings;

namespace MultiTenantsExample.Core.Interfaces;

public interface ITenantService
{
    public string GetDatabaseProvider();
    public string GetConnectionString();
    public Tenant GetTenant();
}
