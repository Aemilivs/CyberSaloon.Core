using System;
using CyberSaloon.Core.Repo.Applications;
using CyberSaloon.Core.Repo.Artists;
using FluentValidation;

namespace CyberSaloon.Core.DTO.Arts.Validation
{
    public class ArtPostDTOValidator : AbstractValidator<ArtPostDTO>
    {
        public ArtPostDTOValidator(
                IApplicationsRepository applicationsRepository,
                IArtistsRepository artistsRepository
            )
        {
            RuleFor(it => it.Summary).NotEmpty().MaximumLength(200);
            RuleFor(it => it.Description).NotEmpty().MaximumLength(1500);
            
            RuleFor(it => it.Url)
                .Must(it => Uri.IsWellFormedUriString(it, UriKind.Absolute))
                .When(it => it.Url.Length > 1)
                .WithMessage("Url is not well formed");
            
            RuleFor(it => it.Application)
                .NotNull()
                .NotEmpty()
                .Must(applicationsRepository.Exists)
                .WithMessage("Application with such id does not exist.");
        }
    }
}