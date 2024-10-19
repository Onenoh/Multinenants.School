using Application.Models.Wrapper;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Schools.Queries
{
    public class GetSchoolByIdQuery : IRequest<IResponseWrapper>
    {
        public int SchoolId { get; set; }
    }

    public class GetSchoolByIdQueryHandler(ISchoolService schoolService) : IRequestHandler<GetSchoolByIdQuery, IResponseWrapper>
    {
        private readonly ISchoolService _schoolService = schoolService;
        public async Task<IResponseWrapper> Handle(GetSchoolByIdQuery request, CancellationToken cancellationToken)
        {
            var schoolInDb = (await _schoolService.GetSchoolByIdAsync(request.SchoolId)).Adapt<SchoolResponse>();

            if (schoolInDb is not null)
            {
                return await ResponseWrapper<SchoolResponse>.SucccessAsync(data: schoolInDb);
            }
            return await ResponseWrapper<SchoolResponse>.FailAsync(message: "School does not exist.");
        }
    }
}
