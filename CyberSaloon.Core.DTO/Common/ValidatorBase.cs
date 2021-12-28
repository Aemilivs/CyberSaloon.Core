using FluentValidation;
using FluentValidation.Results;

namespace CyberSaloon.Core.DTO.Common
{
    public abstract class ValidatorBase : IValidator
    {
        private readonly IValidator<ResourceParameters> _parametersValidator;
        public ValidatorBase(IValidator<ResourceParameters> parametersValidator)
        {
            _parametersValidator =
                parametersValidator ?? 
                throw new System.ArgumentNullException(nameof(parametersValidator));
        }

        public ValidationResult Validate(ResourceParameters parameters) => _parametersValidator.Validate(parameters);
    }
}