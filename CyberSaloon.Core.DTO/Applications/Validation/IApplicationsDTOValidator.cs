using CyberSaloon.Core.DTO.Common;
using CyberSaloon.Core.Repo.Applications;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;

namespace CyberSaloon.Core.DTO.Applications.Validation
{
    public interface IApplicationsDTOValidator : IValidator
    {
        ValidationResult Vaildate(ApplicationGetDTO input);
        ValidationResult Validate(ApplicationPostDTO input);
        ValidationResult Validate(JsonPatchDocument<Application> input);
    }
}