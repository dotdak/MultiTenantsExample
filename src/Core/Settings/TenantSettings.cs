using System.Collections.Generic;

namespace MultiTenantsExample.Core.Settings;

public class TenantSettings
{
    public Configuration? Defaults { get; set; }
    public List<Tenant> Tenants { get; set; } = new List<Tenant>();
}

public class Tenant
{
    public string Name { get; set; } = String.Empty;
    public string TID { get; set; } = String.Empty;
    public string ConnectionString { get; set; } = String.Empty;
}

public class Configuration
{
    public string DBProvider { get; set; } = String.Empty;
    public string ConnectionString { get; set; } = String.Empty;
}
