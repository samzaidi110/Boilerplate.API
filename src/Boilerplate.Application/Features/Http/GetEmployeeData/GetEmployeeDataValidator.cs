using Boilerplate.Application.Common;
using FluentValidation;

namespace Boilerplate.Application.Features.Identity.GetEmployeeData;

public class GetEmployeeDataValidator : AbstractValidator<GetEmployeeDataRequest>
{
    public GetEmployeeDataValidator()
    {
        RuleLevelCascadeMode = ClassLevelCascadeMode;

        RuleFor(x => x.Param1)
            .NotEmpty()
            .MaximumLength(StringSizes.Max);


    }
}