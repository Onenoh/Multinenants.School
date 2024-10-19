using Application.Models.Wrapper;
using Application.Pipelines;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Schools.Commands
{
    public class DeleteSchoolCommand : IRequest<IResponseWrapper>, IValidateMe
    {
        public int SchoolId { get; set; }
    }

    public class DeleteSchoolCommandHandler(ISchoolService schoolService) : IRequestHandler<DeleteSchoolCommand, IResponseWrapper>
    {
        private readonly ISchoolService _schoolService = schoolService;
        public async Task<IResponseWrapper> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
        {
            var schoolId = await _schoolService.GetSchoolByIdAsync(request.SchoolId);
            var deleteSchoolId = await _schoolService.DeleteSchoolAsync(schoolId);

            return await ResponseWrapper<int>.SuccessAsync(data: deleteSchoolId, message: "School deleted successfully.");
        }
    }
}
