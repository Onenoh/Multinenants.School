using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Users.Queries
{
    public class GetUserPermissionsQuery : IRequest<IResponseWrapper>
    {
        public string UserId { get; set; }
    }
    public class GetUserPermissionsQueryHandler(IUserService userService) : IRequestHandler<GetUserPermissionsQuery, IResponseWrapper>
    {
        private readonly IUserService _userService = userService;

        public async Task<IResponseWrapper> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permission = await _userService.GetPermissionsAsync(request.UserId, cancellationToken);
            return await ResponseWrapper<List<string>>.SucccessAsync(data: permission);
        }
    }
}
