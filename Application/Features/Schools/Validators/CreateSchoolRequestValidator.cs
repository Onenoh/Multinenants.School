﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Schools.Validators
{
    internal class CreateSchoolRequestValidator : AbstractValidator<CreateSchoolRequest>
    {
        public CreateSchoolRequestValidator() 
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                    .WithMessage("School name is required.")
                .MaximumLength(60)
                    .WithMessage("School name should not exceed 60 characters length.");
            RuleFor(request => request.EstablishedOn)
                .LessThanOrEqualTo(DateTime.UtcNow)
                    .WithMessage("Date established cannot be future date.");
        }
    }
}
