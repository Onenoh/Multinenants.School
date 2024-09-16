using Application.Exceptions;
using Application.Features.Identity.Users;
using Finbuckle.MultiTenant;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Context;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, ITenantInfo tenant) : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly ApplicationDbContext _context = context;
        private readonly ITenantInfo _tenant = tenant;

        public async Task<string> ActivateOrDeactivateAsync(string userId, bool activation)
        {
            var userInDb = await _userManager.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync() ?? throw new NotFoundExceptions("User does not exists.");

            userInDb.IsActive = activation;

            await _userManager.UpdateAsync(userInDb);

            return userId;
        }

        public async Task<string> AssignRolesAsync(string userId, UserRolesRequest request)
        {
            var userInDb = await _userManager.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync() ?? throw new NotFoundExceptions("User does not exists.");

            if (await _userManager.IsInRoleAsync(userInDb, RoleConstants.Admin) 
                && request.UserRoles.Any(ur => !ur.IsAssigned && ur.Name == RoleConstants.Admin))
            {
                var adminUsersCount = (await _userManager.GetUsersInRoleAsync(RoleConstants.Admin)).Count;
                if (userInDb.Email == TenancyConstants.Root.Email)
                {
                    if (_tenant.Id == TenancyConstants.Root.Id)
                    {
                        throw new ConflictExceptions("Not allowed to remove Admin Role for a Root Tenant user.");
                    }
                }
                else if (adminUsersCount <= 3)
                {
                    throw new ConflictExceptions("Tenant should have at least three admin users.");
                }
            }

            foreach (var userRole in request.UserRoles)
            {
                if (await _roleManager.FindByIdAsync(userRole.RoleId) is not null)
                {
                    if (userRole.IsAssigned)
                    {
                        if (await _userManager.IsInRoleAsync(userInDb, userRole.Name))
                        {
                           await _userManager.AddToRoleAsync(userInDb, userRole.Name);
                        }
                    }
                    else
                    {
                        await _userManager.RemoveFromRoleAsync(userInDb, userRole.Name);
                    }
                }
            }
            return userInDb.Id;
        }

        public Task<string> ChangePasswordsync(ChangePasswordRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<string> CreateUserAsync(CreateUserRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteUserAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDto>> GetUserAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetUserByIdAsync(string userId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserRoleDto>> GetUserRolesAsync(string userId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEmailTakenAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateUserAsync(UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
