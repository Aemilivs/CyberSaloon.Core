using CyberSaloon.Core.DTO.Common;
using CyberSaloon.Core.Repo.Arts;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;

namespace CyberSaloon.Core.DTO.Arts.Validation
{
    public interface IArtsDTOValidator : IValidator
    {
        ValidationResult Vaildate(ArtGetDTO input);
        ValidationResult Validate(ArtPostDTO input);
        ValidationResult Validate(JsonPatchDocument<Art> input);
    }
}