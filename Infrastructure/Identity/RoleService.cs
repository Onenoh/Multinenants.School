using Application.Exceptions;
using Application.Features.Identity.Roles;
using Azure.Core;
using Finbuckle.MultiTenant;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class RoleService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context, ITenantInfo tenant) : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;
        private readonly ITenantInfo _tenant = tenant;

        public async Task<string> CreateAsync(CreateRoleRequest request)
        {
            var newRole = new ApplicationRole()
            {
                Name = request.Name,
                Description = request.Description
            };

            var result = await _roleManager.CreateAsync(newRole);

            if (!result.Succeeded)
            {
                throw new IdentityExceptions("Failed to create a role.", GetIdentityResultErrorDescriptions(result));
            }

            return newRole.Name;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var roleInDb = await _roleManager.FindByIdAsync(id)
                    ?? throw new NotFoundExceptions("Role does not exists.");

            if (RoleConstants.IsDefault(roleInDb.Name))
            {
                throw new ConflictExceptions($"Not allowed to delete '{roleInDb.Name}' role");
            }

            if ((await _userManager.GetUsersInRoleAsync(roleInDb.Name)).Count > 0)
            {
                throw new ConflictExceptions($"Not allowed to delete '{roleInDb.Name}' role is currently assigned to user.");
            }

            var result = await _roleManager.DeleteAsync(roleInDb);
            if (!result.Succeeded)
            {
                throw new IdentityExceptions($"Failed to delete {roleInDb.Name} role.", GetIdentityResultErrorDescriptions(result));
            }

            return roleInDb.Name;
        }

        public async Task<bool> DoesItExistsAsync(string name)
        {
            return await _roleManager.RoleExistsAsync(name);
        }

        public Task<RoleDto> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<RoleDto>> GetRolesAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UpdateAsync(UpdateRoleRequest request)
        {
            var roleInDb = await _roleManager.FindByIdAsync(request.Id)
                    ?? throw new NotFoundExceptions("Role does not exists.");

            if (RoleConstants.IsDefault(roleInDb.Name))
            {
                throw new ConflictExceptions($"Changes not allowed on {roleInDb.Name} role");
            }

            roleInDb.Name = request.Name;
            roleInDb.Description = request.Description;
            roleInDb.NormalizedName = request.Name.ToUpperInvariant();

            var result = await _roleManager.UpdateAsync(roleInDb);

            if (!result.Succeeded)
            {
                throw new IdentityExceptions("Failed to update a role.", GetIdentityResultErrorDescriptions(result));
            }

            return roleInDb.Name;
        }

        public Task<string> UpdatePermissionsAsync(UpdateRolePermissionsRequest request)
        {
            throw new NotImplementedException();
        }

        private List<string> GetIdentityResultErrorDescriptions(IdentityResult identityResult)
        {
            var errorDescriptions = new List<string>();
            foreach (var error in identityResult.Errors)
            {
                errorDescriptions.Add(error.Description);
            }

            return errorDescriptions;
        }
    }
}
