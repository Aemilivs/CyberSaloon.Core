using System;
using CyberSaloon.Core.DTO.Common;
using FluentValidation;
using FluentValidation.Results;

namespace CyberSaloon.Core.DTO.Artists.Validation
{
    public class ArtistsDTOValidator : ValidatorBase, IArtistsDTOValidator
    {
        private readonly IValidator<ArtistGetDTO> _getValidator;
        private readonly IValidator<ArtistPostDTO> _postValidator;
        private readonly IValidator<ArtistPatchDTO> _patchValidator;
        public ArtistsDTOValidator(
                IValidator<ResourceParameters> parametersValidator,
                IValidator<ArtistGetDTO> getValidator,
                IValidator<ArtistPostDTO> postValidator,
                IValidator<ArtistPatchDTO> patchValidator
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

        public ValidationResult Vaildate(ArtistGetDTO input) => _getValidator.Validate(input);
        public ValidationResult Validate(ArtistPostDTO input) => _postValidator.Validate(input);
        public ValidationResult Validate(ArtistPatchDTO input) => _patchValidator.Validate(input);
    }
}