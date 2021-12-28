using System.Linq;
using CyberSaloon.Core.Repo.Applications;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;

namespace CyberSaloon.Core.DTO.Applications.Validation
{
    public class ApplicationPatchDTOValidator : AbstractValidator<JsonPatchDocument<Application>>
    {
        public ApplicationPatchDTOValidator(IApplicationsRepository applicationsRepository)
        {
            RuleFor(it => it).NotNull();
            RuleForEach(it => it.Operations)
                .ChildRules(
                    operations =>
                    {
                        operations
                            .RuleFor(opeartion => opeartion.path)
                            .Must(path => path == "/Summary" || path == "/Description")
                            .WithMessage("Patch document operations refer to invalid paths.");

                        operations
                            .RuleFor(opeartion => opeartion.value as string)
                            .NotNull()
                            .NotEmpty()
                            .MaximumLength(50)
                            .When(operation => operation.path == "/Summary");

                        operations
                            .RuleFor(opeartion => opeartion.value as string)
                            .NotNull()
                            .NotEmpty()
                            .MaximumLength(5000)
                            .When(operation => operation.path == "/Description");
                    }
                );
        }
    }
}