using System;
using System.Linq;
using CyberSaloon.Core.Repo.Artists;
using FluentValidation;

namespace CyberSaloon.Core.DTO.Artists.Validation
{
    public class ArtistPostDTOValidator : AbstractValidator<ArtistPostDTO>
    {
        public ArtistPostDTOValidator(
                IArtistsRepository artistsRepository    
            )
        {
            RuleFor(it => it).NotNull();
            RuleFor(it => it.Alias)
                .NotEmpty()
                .MaximumLength(50)
                .Must(it => it.All(char.IsLetterOrDigit))
                .WithMessage("Alias must be a string with only letters and digits")
                .Must(it => artistsRepository.ReadByAlias(it) == null)
                .WithMessage("Artist with such alias already exists.");
            RuleFor(it => it.Description)
                .MaximumLength(1500);
            RuleFor(it => it.PhotoUrl)
                .MaximumLength(500)
                .Must(it => Uri.IsWellFormedUriString(it, UriKind.Absolute))
                .When(it => it.PhotoUrl.Length > 1)
                .WithMessage("Url is not well formed");
        }
    }
}