using Infrastructure.Tenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.DbInitializers
{
    internal class TenantDbInitializer(TenantDbContext tenantDbContext) : ITenantDbInitializer
    {
        private readonly TenantDbContext _tenantDbContext = tenantDbContext;
        public Task InitializeApplicationDbForTenantAsync(SchoolTenantInfo tenant, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task InitializeDatabaseWithTenantAsync(CancellationToken cancellationToken)
        {
            if (await _tenantDbContext.TenantInfo.FindAsync([TenancyConstants.Root.Id], cancellationToken: cancellationToken) is null)
            {
                var rootTenant = new SchoolTenantInfo
                {
                    Id = TenancyConstants.Root.Id,
                    Identifier = TenancyConstants.Root.Name,
                    Name = TenancyConstants.Root.Name,
                    AdminEmail = TenancyConstants.Root.Email,
                    IsActive = true,
                    ValidUpTo = DateTime.UtcNow.AddYears(1)
                };

                await _tenantDbContext.TenantInfo.AddAsync(rootTenant, cancellationToken);
                await _tenantDbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
