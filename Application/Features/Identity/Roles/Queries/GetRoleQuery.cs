using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Roles.Queries
{
    public class GetRoleQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetRolesQueryHandler(IRoleService roleService) : IRequestHandler<GetRoleQuery, IResponseWrapper>
    {
        private readonly IRoleService _roleService = roleService;
        public async Task<IResponseWrapper> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleService.GetRolesAsync(cancellationToken);
            return await ResponseWrapper<List<RoleDto>>.SucccessAsync(data: roles);
        }
    }
}
