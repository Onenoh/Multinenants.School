using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Roles.Queries
{
    public class GetRoleWithPermissionQuery : IRequest<IResponseWrapper>
    {
        public string RoleId { get; set; }
    }

    public class GetRoleWithPermissionQueryHandler(IRoleService roleService) : IRequestHandler<GetRoleWithPermissionQuery, IResponseWrapper>
    {
        private readonly IRoleService _roleService = roleService;
        public async Task<IResponseWrapper> Handle(GetRoleWithPermissionQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleService.GetRoleWithPermissionAsync(request.RoleId, cancellationToken);
            return await ResponseWrapper<RoleDto>.SucccessAsync(data: role);
        }
    }
}
