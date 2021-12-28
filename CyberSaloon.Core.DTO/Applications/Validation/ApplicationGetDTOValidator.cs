using FluentValidation;

namespace CyberSaloon.Core.DTO.Applications.Validation
{
    public class ApplicationGetDTOValidator : AbstractValidator<ApplicationGetDTO>
    {
        public ApplicationGetDTOValidator()
        {
            RuleFor(it => it).NotNull();
            RuleFor(it => it.Id).NotNull();
            RuleFor(it => it.Artist).NotNull();
        }
    }
}