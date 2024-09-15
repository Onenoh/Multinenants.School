using Application.Exceptions;
using Application.Features.Identity.Roles;
using Finbuckle.MultiTenant;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Context;
using Infrastructure.Tenancy;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


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

        public async Task<RoleDto> GetRoleByIdAsync(string id, CancellationToken ct)
        {
            var roleInDb = await _context.Roles
                .SingleOrDefaultAsync(r => r.Id == id, ct) 
                ?? throw new NotFoundExceptions("Role does not exists.");

            return roleInDb.Adapt<RoleDto>();

        }

        public async Task<List<RoleDto>> GetRolesAsync(CancellationToken ct)
        {
            var roleInDb = await _roleManager.Roles.ToListAsync(ct);

            return roleInDb.Adapt<List<RoleDto>>();
        }

        public async Task<RoleDto> GetRoleWithPermissionAsync(string id, CancellationToken ct)
        {
            var roleDto = await GetRoleByIdAsync(id, ct);
            roleDto.Permissions = await _context.RoleClaims
                .Where(rc => rc.RoleId == id && rc.ClaimType == ClaimConstants.Permission)
                .Select(rc => rc.ClaimValue)
                .ToListAsync(ct);
            return roleDto;
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

        public async Task<string> UpdatePermissionsAsync(UpdateRolePermissionsRequest request)
        {
            var roleInDb = await _roleManager.FindByIdAsync(request.RoleId)
                ?? throw new NotFoundExceptions("Role does not exists.");

            if (roleInDb.Name == RoleConstants.Admin)
            {
                throw new ConflictExceptions($"Not allowed to change permissions for {roleInDb.Name} role");
            }

            if (_tenant.Id != TenancyConstants.Root.Id)
            {
                request.Permissions.RemoveAll(p => p.StartsWith("Permission.Root."));
            }

            var currentClaims = await _roleManager.GetClaimsAsync(roleInDb);

            foreach (var claim in currentClaims.Where(c => !request.Permissions.Any(p => p == c.Value)))
            {
                var result = await _roleManager.RemoveClaimAsync(roleInDb, claim);

                if (!result.Succeeded)
                {
                    throw new IdentityExceptions("Failed to remove permission.", GetIdentityResultErrorDescriptions(result));
                }
            }

            foreach(var permission in request.Permissions.Where(p => !currentClaims.Any(c => c.Value == p)))
            {
                await _context.RoleClaims
                    .AddAsync(new IdentityRoleClaim<string>
                    {
                        RoleId = roleInDb.Id,
                        ClaimType = ClaimConstants.Permission,
                        ClaimValue = permission
                    });
                await _context.SaveChangesAsync();
            }

            return "Permissions Updated Successfully";
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
