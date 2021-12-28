using System;
using CyberSaloon.Core.DTO.Common;
using FluentValidation;
using FluentValidation.Results;

namespace CyberSaloon.Core.DTO.Applicants.Validation
{
    public class ApplicantsDTOValidator : ValidatorBase, IApplicantsDTOValidator
    {
        private readonly IValidator<ApplicantGetDTO> _getValidator;
        private readonly IValidator<ApplicantPostDTO> _postValidator;
        private readonly IValidator<ApplicantPatchDTO> _patchValidator;
        public ApplicantsDTOValidator(
                IValidator<ResourceParameters> parametersValidator,
                IValidator<ApplicantGetDTO> getValidator,
                IValidator<ApplicantPostDTO> postValidator,
                IValidator<ApplicantPatchDTO> patchValidator
            ) : base (parametersValidator)
        {
            _getValidator = 
                getValidator ?? 
                throw new ArgumentNullException(nameof(getValidator));

            _postValidator = 
                postValidator ?? 
                throw new ArgumentNullException(nameof(postValidator));

            _patchValidator =
                patchValidator ??
                throw new ArgumentNullException(nameof(patchValidator));
        }

        public ValidationResult Validate(ApplicantGetDTO input) => _getValidator.Validate(input);
        public ValidationResult Validate(ApplicantPostDTO input) => _postValidator.Validate(input);
        public ValidationResult Validate(ApplicantPatchDTO input) => _patchValidator.Validate(input);
    }
}