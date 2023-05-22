using Microsoft.EntityFrameworkCore;
using MultiTenantsExample.Core.Contracts;
using MultiTenantsExample.Core.Entities;
using MultiTenantsExample.Core.Interfaces;

namespace MultiTenantsExample.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public string? TenantId { get; set; }
    private readonly ITenantService _tenantService;

    public DbSet<Product> Products { get; set; }

    public ApplicationDbContext(DbContextOptions options, ITenantService tenantService) : base(options)
    {
        _tenantService = tenantService;
        TenantId = _tenantService?.GetTenant()?.TID;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().HasQueryFilter(i => i.TenantId == TenantId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var tenantConnectionString = _tenantService.GetConnectionString();
        if (!string.IsNullOrEmpty(tenantConnectionString))
        {
            var dBProvider = _tenantService.GetDatabaseProvider();
            if (dBProvider?.ToLower() == "mssql")
            {
                optionsBuilder.UseSqlServer(_tenantService.GetConnectionString());
            }
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                case EntityState.Modified:
                    entry.Entity.TenantId = TenantId;
                    break;
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}
