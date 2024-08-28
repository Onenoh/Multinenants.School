using Application.Features.Tenancy.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tenancy
{
    public interface ITenantService
    {
        Task<string> CreateTenantAsync(CreateTenantRequest createTenant);
    }
}
