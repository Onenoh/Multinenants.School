using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Roles
{
    public interface IRoleService
    {
        Task<string> CreateAsync(CreateRoleRequest request);
        Task<string> UpdateAsync(UpdateRoleRequest request);
        Task<string> DeleteAsync(string id);
        Task<string> UpdatePermissionsAsync(UpdateRolePermissionsRequest request);

        Task<List<RoleDto>> GetRolesAsync(CancellationToken ct);
        Task<RoleDto> GetByIdAsync(string id);
        Task<bool> DoesItExistsAsync(string name);
    }
}
