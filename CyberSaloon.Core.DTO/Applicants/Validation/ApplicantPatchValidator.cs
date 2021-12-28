using CyberSaloon.Core.Repo.Applicants;
using FluentValidation;
using System.Linq;

namespace CyberSaloon.Core.DTO.Applicants.Validation
{
    public class ApplicantPatchDTOValidator : AbstractValidator<ApplicantPatchDTO>
    {
        public ApplicantPatchDTOValidator(IApplicantsRepository applicantRepository)
        {
            RuleFor(it => it).NotNull();
            RuleFor(it => it.Id)
                .NotEmpty()
                .Must(applicantRepository.Exists)
                .WithMessage("Application with such id does not exist.");
            RuleFor(it => it.Alias)
                .NotEmpty()
                .MaximumLength(50)
                .Must(it => it.All(char.IsLetterOrDigit))
                .WithMessage("Alias must be a string with only letters and digits");
        }
    }
}