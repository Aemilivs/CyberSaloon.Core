using CyberSaloon.Core.DTO.Common;
using FluentValidation.Results;

namespace CyberSaloon.Core.DTO.Applicants.Validation
{
    public interface IApplicantsDTOValidator : IValidator
    {
        ValidationResult Validate(ApplicantGetDTO input);
        ValidationResult Validate(ApplicantPostDTO input);
        ValidationResult Validate(ApplicantPatchDTO input);
    }
}