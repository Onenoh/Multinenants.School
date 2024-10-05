using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Token.Queries
{
    public class GetRefreshTokenQuery : IRequest<IResponseWrapper>
    {
        public RefreshTokenRequest RefreshToken { get; set; }
    }

    public class GetRefreshTokenQueryHandler(ITokenService tokenService) : IRequestHandler<GetRefreshTokenQuery, IResponseWrapper>
    {
        private readonly ITokenService _tokenService = tokenService;

        public async Task<IResponseWrapper> Handle(GetRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var refreshToken = await _tokenService.RefreshTokenAsync(request.RefreshToken);
            return await ResponseWrapper<TokenResponse>.SucccessAsync(refreshToken);
        }
    }

}
