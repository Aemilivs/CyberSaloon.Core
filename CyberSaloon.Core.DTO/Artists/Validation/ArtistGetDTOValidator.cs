using FluentValidation;

namespace CyberSaloon.Core.DTO.Artists.Validation
{
    public class ArtistGetDTOValidator : AbstractValidator<ArtistGetDTO>
    {
        public ArtistGetDTOValidator()
        {
            RuleFor(it => it).NotNull();
            RuleFor(it => it.Id).NotEmpty();
        }
    }
}