using Application.Features.Tenancy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tenancy
{
    public interface ITenantService
    {
        Task<string> CreateTenantAsync(CreateTenantRequest createTenant, CancellationToken ct);
        Task<string> ActiveAsync(string id);
        Task<string> DeactivateAsync(string id);
        Task<string> UpdateSubscriptionAsync(string id, DateTime newExpiryDate);

        Task<List<TenantDto>> GetTenantsAsync();
        Task<TenantDto> GetTenantByIdAsync(string id);
    }
}
