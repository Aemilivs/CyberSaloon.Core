using CyberSaloon.Core.Repo.Applicants;
using FluentValidation;
using System.Linq;

namespace CyberSaloon.Core.DTO.Applicants.Validation
{
    public class ApplicantPostDTOValidator : AbstractValidator<ApplicantPostDTO>
    {
        public ApplicantPostDTOValidator(
                IApplicantsRepository applicantRepository
            )
        {
            RuleFor(it => it).NotNull();
            RuleFor(it => it.Alias)
                .NotEmpty()
                .MaximumLength(50)
                .Must(it => it.All(char.IsLetterOrDigit))
                .WithMessage("Alias must be a string with only letters and digits")
                .Must(it => applicantRepository.ReadByAlias(it) == null)
                .WithMessage("Artist with such alias already exists.");
        }
    }
}