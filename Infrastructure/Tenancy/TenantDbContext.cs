using Finbuckle.MultiTenant.Stores;
using Infrastructure.Persistence.DbConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tenancy
{
    public class TenantDbContext(DbContextOptions<TenantDbContext> options) : EFCoreStoreDbContext<SchoolTenantInfo>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SchoolTenantInfo>()
                .ToTable("Tenants", SchemaNames.Multitenancy);
        }
    }
}
