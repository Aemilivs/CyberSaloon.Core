using System;
using CyberSaloon.Core.DTO.Common;
using CyberSaloon.Core.Repo.Applications;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;

namespace CyberSaloon.Core.DTO.Applications.Validation
{
    public class ApplicationsDTOValidator : ValidatorBase, IApplicationsDTOValidator
    {
        private readonly IValidator<ApplicationGetDTO> _getValidator;
        private readonly IValidator<ApplicationPostDTO> _postValidator;
        private readonly IValidator<JsonPatchDocument<Application>> _patchValidator;
        public ApplicationsDTOValidator(
                IValidator<ResourceParameters> parametersValidator,
                IValidator<ApplicationGetDTO> getValidator,
                IValidator<ApplicationPostDTO> postValidator,
                IValidator<JsonPatchDocument<Application>> patchValidator
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

        public ValidationResult Vaildate(ApplicationGetDTO input) => _getValidator.Validate(input);
        public ValidationResult Validate(ApplicationPostDTO input) => _postValidator.Validate(input);
        public ValidationResult Validate(JsonPatchDocument<Application> input) => _patchValidator.Validate(input);
    }
}