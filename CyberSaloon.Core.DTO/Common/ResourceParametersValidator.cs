using FluentValidation;

namespace CyberSaloon.Core.DTO.Common
{
    public class ResourceParametersValidator : AbstractValidator<ResourceParameters>
    {
        public ResourceParametersValidator()
        {
            RuleFor(it => it.PageNumber).GreaterThan(0);
            RuleFor(it => it.PageSize).GreaterThan(0);
            RuleFor(it => it.PageSize).LessThanOrEqualTo(100);
        }
    }
}