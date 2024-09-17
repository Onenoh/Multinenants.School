using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Users
{
    public interface IUserService
    {
        Task<string> CreateUserAsync(CreateUserRequest request);
        Task<string> UpdateUserAsync(UpdateUserRequest request);
        Task<string> DeleteUserAsync(string userId);
        Task<string> ActivateOrDeactivateAsync(string userId, bool activation);
        Task<string> ChangePasswordsync(ChangePasswordRequest request);
        Task<string> AssignRolesAsync(string userId, UserRolesRequest request);
        Task<List<UserDto>> GetUserAsync(CancellationToken ct);
        Task<UserDto> GetUserByIdAsync(string userId, CancellationToken ct);
        Task<List<UserRoleDto>> GetUserRolesAsync(string userId, CancellationToken ct);
        Task<bool> IsEmailTakenAsync(string email);
    }
}
