using CyberSaloon.Core.Repo.Applicants;
using CyberSaloon.Core.Repo.Artists;
using FluentValidation;

namespace CyberSaloon.Core.DTO.Applications.Validation
{
    public class ApplicationPostDTOValidator : AbstractValidator<ApplicationPostDTO>
    {
        public ApplicationPostDTOValidator(
                IApplicantsRepository applicantsRepository,
                IArtistsRepository artistsRepository
            )
        {
            RuleFor(it => it.Summary).NotEmpty().NotNull();
            RuleFor(it => it.Description).NotEmpty().NotNull();
            RuleFor(it => it.Author)
                .NotNull()
                .Must(applicantsRepository.Exists)
                .WithMessage("Applicant with such id doesn't exist.");
                
            RuleFor(it => it.Artist)
                .NotNull()
                .Must(artistsRepository.Exists)
                .WithMessage("Artist with such id doesn't exist.");
        }
    }
}