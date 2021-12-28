using System;
using CyberSaloon.Core.Repo.Arts;
using CyberSaloon.Core.Repo.Applications;
using CyberSaloon.Core.Repo.Artists;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;

namespace CyberSaloon.Core.DTO.Arts.Validation
{
    public class ArtPatchDTOValidator : AbstractValidator<JsonPatchDocument<Art>>
    {
        public ArtPatchDTOValidator(
                IArtsRepository artsRepository,
                IApplicationsRepository applicationsRepository,
                IArtistsRepository artistsRepository
            )
        {
            RuleFor(it => it).NotNull();
            RuleForEach(it => it.Operations)
                .ChildRules(
                    operations =>
                    {
                        operations
                            .RuleFor(opeartion => opeartion.path)
                            .Must(path => 
                                    path == "/Summary" || 
                                    path == "/Description" ||
                                    path == "/Url"
                                )
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

                        operations
                            .RuleFor(opeartion => opeartion.value as string)
                            .Must(it =>  Uri.IsWellFormedUriString(it, UriKind.Absolute))
                            .When(operation => operation.path == "/Url")
                            .WithMessage("Url is not well formed");
                    }
                );
        }
    }
}