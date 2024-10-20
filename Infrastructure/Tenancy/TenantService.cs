using Application.Features.Tenancy;
using Application.Features.Tenancy.Models;
using Finbuckle.MultiTenant;
using Infrastructure.Persistence.DbInitializers;
using Mapster;

namespace Infrastructure.Tenancy
{
    public class TenantService(IMultiTenantStore<SchoolTenantInfo> tenantStore, ApplicationDbInitializer applicationDbInitializer) : ITenantService
    {
        private readonly IMultiTenantStore<SchoolTenantInfo> _tenantStore = tenantStore;
        private readonly ApplicationDbInitializer _applicationDbInitializer = applicationDbInitializer;

        public async Task<string> ActiveAsync(string id)
        {
            var tenantInDb = await _tenantStore.TryGetAsync(id);
            tenantInDb.IsActive = true;

            await _tenantStore.TryUpdateAsync(tenantInDb);
            return tenantInDb.Id;
        }

        public async Task<string> CreateTenantAsync(CreateTenantRequest createTenant, CancellationToken ct)
        {
            var newTenant = new SchoolTenantInfo
            {
                Id = createTenant.Identifier,
                Name = createTenant.Name,
                ConnectionString = createTenant.ConnectionString,
                AdminEmail = createTenant.AdminEmail,
                ValidUpTo = createTenant.ValidUpTo,
                IsActive = createTenant.IsActive
            };

            await _tenantStore.TryAddAsync(newTenant);

            try
            {
                await _applicationDbInitializer.InitializeDatabaseAsync(ct);
            }
            catch (Exception) 
            {
                await _tenantStore.TryRemoveAsync(createTenant.Identifier);
                throw;
            }

            return newTenant.Id;
        }

        public async Task<string> DeactivateAsync(string id)
        {
            var tenantInDb = await _tenantStore.TryGetAsync(id);
            tenantInDb.IsActive = false;

            await _tenantStore.TryUpdateAsync(tenantInDb);
            return tenantInDb.Id;
        }

        public async Task<TenantDto> GetTenantByIdAsync(string id)
        {
            var tenantInDb = await _tenantStore.TryGetAsync(id);

            //return new TenantDto
            //{
            //    Id = tenantInDb.Id,
            //    Name = tenantInDb.Name,
            //    AdminEmail = tenantInDb.AdminEmail,
            //    ConnectionString = tenantInDb.ConnectionString,
            //    ValidUpTo = tenantInDb.ValidUpTo,
            //    IsActive = tenantInDb.IsActive
            //};

            return tenantInDb.Adapt<TenantDto>();
        }

        public async Task<List<TenantDto>> GetTenantsAsync()
        {
            var tenantsInDb = (await _tenantStore.GetAllAsync()).Adapt<List<TenantDto>>();
            return tenantsInDb.Adapt<List<TenantDto>>();
        }

        public async Task<string> UpdateSubscriptionAsync(string id, DateTime newExpiryDate)
        {
            var tenantInDb = await _tenantStore.TryGetAsync(id);
            tenantInDb.ValidUpTo = newExpiryDate;
            return tenantInDb.Id;
        }
    }
}
