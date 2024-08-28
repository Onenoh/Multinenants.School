using Application.Features.Tenancy;
using Application.Features.Tenancy.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tenancy
{
    public class TenantService : ITenantService
    {
        public Task<string> CreateTenantAsync(CreateTenantRequest createTenant)
        {
            throw new NotImplementedException();
        }
    }
}
