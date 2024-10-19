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
    public class GetSchoolByNameQuery : IRequest<IResponseWrapper>
    {
        public string Name { get; set; }
    }

    public class GetSchoolByNameQueryHandler(ISchoolService schoolService) : IRequestHandler<GetSchoolByNameQuery, IResponseWrapper>
    {
        private readonly ISchoolService _schoolService = schoolService;

        public async Task<IResponseWrapper> Handle(GetSchoolByNameQuery request, CancellationToken cancellationToken)
        {
            var schoolInDb = (await _schoolService.GetSchoolByNameAsync(request.Name)).Adapt<SchoolResponse>();

            if (schoolInDb is not null)
            {
                return await ResponseWrapper<SchoolResponse>.SucccessAsync(data: schoolInDb);
            }
            return await ResponseWrapper<SchoolResponse>.FailAsync(message: "School does not exist.");
            
        }
    }
}
