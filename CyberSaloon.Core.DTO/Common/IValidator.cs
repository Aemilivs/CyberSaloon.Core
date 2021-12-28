using FluentValidation.Results;

namespace CyberSaloon.Core.DTO.Common
{
    // Feeling awkward, because of this, but ok
    public interface IValidator
    {
        ValidationResult Validate(ResourceParameters parameters);
    }
}