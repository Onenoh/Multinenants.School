using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Token
{
    public interface ITokenService
    {
        Task<TokenResponse> LoginAsync(TokenRequest request);
        Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
