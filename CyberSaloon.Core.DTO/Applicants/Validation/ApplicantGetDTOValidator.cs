using FluentValidation;
using System.Linq;

namespace CyberSaloon.Core.DTO.Applicants.Validation
{
    public class ApplicantGetDTOValidator : AbstractValidator<ApplicantGetDTO>
    {
        public ApplicantGetDTOValidator()
        {
            RuleFor(it => it).NotNull();
            RuleFor(it => it.Id).NotEmpty();
            RuleFor(it => it.Alias)
                .NotEmpty()
                .MaximumLength(50)
                .Must(it => it.All(char.IsLetterOrDigit))
                .WithMessage("Alias must be a string with only letters and digits");
        }
    }
}