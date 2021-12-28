using CyberSaloon.Core.DTO.Common;
using FluentValidation.Results;

namespace CyberSaloon.Core.DTO.Artists.Validation
{
    public interface IArtistsDTOValidator : IValidator
    {
        ValidationResult Vaildate(ArtistGetDTO input);
        ValidationResult Validate(ArtistPostDTO input);
        ValidationResult Validate(ArtistPatchDTO input);
    }
}