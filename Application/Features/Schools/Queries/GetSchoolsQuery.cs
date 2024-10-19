using Application.Models.Wrapper;
using Domain.Entities;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Schools.Queries
{
    public class GetSchoolsQuery : IRequest<IResponseWrapper>
    {
    }

    public class GetSchoolsQueryHandler(ISchoolService schoolService) : IRequestHandler<GetSchoolsQuery, IResponseWrapper>
    {
        private readonly ISchoolService _schoolService = schoolService;

        public async Task<IResponseWrapper> Handle(GetSchoolsQuery request, CancellationToken cancellationToken)
        {
            var schoolsInDb = await _schoolService.GetSchoolAsync();

            if (schoolsInDb.Count > 0)
            {
                return await ResponseWrapper<List<SchoolResponse>>.SucccessAsync(data: schoolsInDb.Adapt<List<SchoolResponse>>());
            }
            return await ResponseWrapper<List<SchoolResponse>>.FailAsync(message: "No Schools were found.");
        }
    }
}
