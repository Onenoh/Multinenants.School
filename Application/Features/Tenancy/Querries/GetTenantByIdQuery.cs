using Application.Features.Tenancy.Models;
using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tenancy.Querries
{
    public class GetTenantByIdQuery : IRequest<IResponseWrapper>
    {
        public string TenantId { get; set; }
    }

    public class GetTenantByIdQueryHandler(ITenantService tenantService) : IRequestHandler<GetTenantByIdQuery, IResponseWrapper>
    {
        private readonly ITenantService _tenantService = tenantService;

        public async Task<IResponseWrapper> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
        {
            var tenantInDb = await _tenantService.GetTenantByIdAsync(request.TenantId);
            if (tenantInDb is not null)
            {
               return await ResponseWrapper<TenantDto>.SucccessAsync(data: tenantInDb);
            }
            return await ResponseWrapper<TenantDto>.FailAsync(message: "Tenant does not exist.");
        }
    }
}
