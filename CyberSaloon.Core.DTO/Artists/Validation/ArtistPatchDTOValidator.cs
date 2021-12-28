using CyberSaloon.Core.Repo.Artists;
using FluentValidation;
using System;
using System.Linq;

namespace CyberSaloon.Core.DTO.Artists.Validation
{
    public class ArtistPatchDTOValidator : AbstractValidator<ArtistPatchDTO>
    {
        public ArtistPatchDTOValidator(IArtistsRepository artistsRepository)
        {
            RuleFor(it => it).NotNull();
            RuleFor(it => it.Id)
                .NotEmpty()
                .Must(artistsRepository.Exists)
                .WithMessage("Artist with such Id doesn't exist");

            RuleFor(it => it.Alias)
                .NotEmpty()
                .MaximumLength(50)
                .Must(it => it.All(char.IsLetterOrDigit))
                .WithMessage("Alias must be a string of letters and digits");

            RuleFor(it => it.Description).NotEmpty().MaximumLength(1500);
            RuleFor(it => it.PhotoUrl)
                .MaximumLength(500)
                .Must(it => Uri.IsWellFormedUriString(it, UriKind.Absolute))
                .When(it => it.PhotoUrl.Length > 1)
                .WithMessage("Url is not well formed");
        }
    }
}