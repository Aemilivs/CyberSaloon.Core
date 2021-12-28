using System;
using CyberSaloon.Core.DTO.Common;
using CyberSaloon.Core.Repo.Arts;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;

namespace CyberSaloon.Core.DTO.Arts.Validation
{
    public class ArtsDTOValidator : ValidatorBase, IArtsDTOValidator
    {
        private readonly IValidator<ArtGetDTO> _getValidator;
        private readonly IValidator<ArtPostDTO> _postValidator;
        private readonly IValidator<JsonPatchDocument<Art>> _patchValidator;
        public ArtsDTOValidator(
                IValidator<ResourceParameters> parametersValidator,
                IValidator<ArtGetDTO> getValidator,
                IValidator<ArtPostDTO> postValidator,
                IValidator<JsonPatchDocument<Art>> patchValidator
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

        public ValidationResult Vaildate(ArtGetDTO input) => _getValidator.Validate(input);
        public ValidationResult Validate(ArtPostDTO input) => _postValidator.Validate(input);
        public ValidationResult Validate(JsonPatchDocument<Art> input) => _patchValidator.Validate(input);
    }
}