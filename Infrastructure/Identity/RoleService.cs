using Application.Features.Identity.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class RoleService : IRoleService
    {
        public Task<string> CreateAsync(CreateRoleRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DoesItExistsAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<RoleDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<RoleDto>> GetRolesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateAsync(UpdateRoleRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdatePermissionsAsync(UpdateRolePermissionsRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
