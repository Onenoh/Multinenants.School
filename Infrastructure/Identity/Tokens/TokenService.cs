using Application.Features.Identity.Token;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Infrastructure.Identity.Tokens
{
    public class TokenService(UserManager<ApplicationUser> userManger, SchoolTenantInfo tenant) : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManger;
        private readonly SchoolTenantInfo _tenant = tenant;
        public async Task<TokenResponse> LoginAsync(TokenRequest request)
        {
           var userInDb = await _userManager.FindByEmailAsync(request.Email);

            if (userInDb is null)
            {
                
            }

            if (!await _userManager.CheckPasswordAsync(userInDb, request.Password))
            {

            }

            if (!userInDb.IsActive)
            {

            }

            if (_tenant.Id != TenancyConstants.Root.Id)
            {
                if (_tenant.ValidUpTo < DateTime.UtcNow)
                {

                }
            }
        }

        public Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            throw new NotImplementedException();
        }

        private async Task<TokenResponse> GenerateTokenAndUpdateUserAsync(ApplicationUser user)
        {
            string newToken = GenerateJwt(user);
        }

        private string GenerateJwt(ApplicationUser user) 
        {
            return GenerateEncryptedToken(GetSigningCredentials(), GetUserClaims(user));
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private SigningCredentials GetSigningCredentials()
        {
            byte[] secret = Encoding.UTF8.GetBytes("School@UDEMYLesson!engineer");
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }

        private IEnumerable<Claim> GetUserClaims(ApplicationUser user)
        {
            return
            [
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.FirstName),
                new(ClaimTypes.Surname, user.LastName),
                new(ClaimConstants.Tenant, _tenant.Id),
                new(ClaimTypes.MobilePhone, user.PhoneNumber)
            ];
        }
    }
}
