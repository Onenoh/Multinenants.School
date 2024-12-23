﻿using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Context;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.DbInitializers
{
    public class ApplicationDbInitializer(SchoolTenantInfo tenant, ApplicationDbContext applicationDbContext, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        private readonly SchoolTenantInfo _tenant = tenant;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;


        public async Task InitializeDatabaseAsync(CancellationToken cancellationToken)
        {
            if (_applicationDbContext.Database.GetMigrations().Any())
            {
                if ((await _applicationDbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                {
                    await _applicationDbContext.Database.MigrateAsync(cancellationToken);
                }

                if (await _applicationDbContext.Database.CanConnectAsync(cancellationToken))
                {
                    await InitializerDefaultRoleAsync(cancellationToken);
                    await InitializeAdminUserAsync();
                }
            }
            
        }

        private async Task InitializerDefaultRoleAsync(CancellationToken cancellationToken)
        {
            foreach (string roleName in RoleConstants.DefaultRoles)
            {
                if (await _roleManager.Roles.SingleOrDefaultAsync(role => role.Name == roleName, cancellationToken) is not ApplicationRole incomingRole)
                {
                    incomingRole = new ApplicationRole() { Name = roleName, Description = $"{roleName} Role" };
                    await _roleManager.CreateAsync(incomingRole);
                }

                if (roleName == RoleConstants.Basic)
                {
                    await AssignPermissionsToRole(SchoolPermissions.Basic, incomingRole, cancellationToken);
                }
                else if (roleName == RoleConstants.Admin)
                {
                    await AssignPermissionsToRole(SchoolPermissions.Admin, incomingRole, cancellationToken);
                    if (_tenant.Id == TenancyConstants.Root.Id)
                    {
                        await AssignPermissionsToRole(SchoolPermissions.Root, incomingRole, cancellationToken);
                    }
                }
            }
        }

        private async Task InitializeAdminUserAsync()
        {
            if (string.IsNullOrEmpty(_tenant.AdminEmail))
            {
                return;
            }

            if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == _tenant.AdminEmail) is not ApplicationUser adminUser)
            {
                adminUser = new ApplicationUser()
                {
                    FirstName = TenancyConstants.FirstName,
                    LastName = TenancyConstants.LastName,
                    Email = _tenant.AdminEmail,
                    UserName = _tenant.AdminEmail,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedEmail = _tenant.AdminEmail.ToUpperInvariant(),
                    NormalizedUserName = _tenant.AdminEmail.ToUpperInvariant(),
                    IsActive = true
                };

                var password = new PasswordHasher<ApplicationUser>();
                adminUser.PasswordHash = password.HashPassword(adminUser, TenancyConstants.DefaultPassword);
                await _userManager.CreateAsync(adminUser);
            }

            if (!await _userManager.IsInRoleAsync(adminUser, RoleConstants.Admin))
            {
                await _userManager.AddToRoleAsync(adminUser, RoleConstants.Admin);
            }
        }

        private async Task AssignPermissionsToRole(IReadOnlyList<SchoolPermission> rolePermissions, ApplicationRole currentRole, CancellationToken cancellationToken)
        {
            var currentClaims = await _roleManager.GetClaimsAsync(currentRole);

            foreach (var rolePermission in rolePermissions)
            {
                if (!currentClaims.Any(c => c.Type == ClaimConstants.Permission && c.Value == rolePermission.Name))
                {
                    await _applicationDbContext.RoleClaims.AddAsync(new IdentityRoleClaim<string>
                    {
                        RoleId = currentRole.Id,
                        ClaimType = ClaimConstants.Permission,
                        ClaimValue = rolePermission.Name
                    }, cancellationToken);

                    await _applicationDbContext.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
    
}
