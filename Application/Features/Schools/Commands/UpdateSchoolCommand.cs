using Application.Models.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Schools.Commands
{
    public class UpdateSchoolCommand : IRequest<IResponseWrapper>
    {
        public UpdateSchoolRequest SchoolRequest { get; set; }
    }

    public class UpdateSchoolCommandHandler(ISchoolService schoolService) : IRequestHandler<UpdateSchoolCommand, IResponseWrapper>
    {
        private readonly ISchoolService _schoolService = schoolService;
        public async Task<IResponseWrapper> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {
            var schoolInDb = await _schoolService.GetSchoolByIdAsync(request.SchoolRequest.Id);

            schoolInDb.Name = request.SchoolRequest.Name;
            schoolInDb.EstablishedOn = request.SchoolRequest.EstablishedOn;

            var updateSchoolId = await _schoolService.UpdateSchoolAsync(schoolInDb);

            return await ResponseWrapper<int>.SuccessAsync(data: updateSchoolId, message: "school updated successfully.");
        }
    }
}
