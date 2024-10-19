using Application.Models.Wrapper;
using Application.Pipelines;
using Domain.Entities;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Schools.Commands
{
    public class CreateSchoolCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public CreateSchoolRequest SchoolRequest { get; set; }
    }

    public class CreateSchoolCommandHandler(ISchoolService schoolService) : IRequestHandler<CreateSchoolCommand, IResponseWrapper>
    {
        private readonly ISchoolService _schoolService = schoolService;
        public async Task<IResponseWrapper> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
        {
            var schoolId = await _schoolService.CreateSchoolAsync(request.SchoolRequest.Adapt<School>());

            return await ResponseWrapper<int>.SuccessAsync(data: schoolId, message: "School created successfully.");

        }
    }
}
