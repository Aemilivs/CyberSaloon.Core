using CyberSaloon.Core.BLL.Artists;
using CyberSaloon.Core.Repo.Artists;
using CyberSaloon.Core.DTO.Artists;
using CyberSaloon.Core.DTO.Artists.Validation;
using CyberSaloon.Core.BLL.Arts;
using CyberSaloon.Core.Repo.Arts;
using CyberSaloon.Core.DTO.Arts;
using CyberSaloon.Core.DTO.Arts.Validation;
using CyberSaloon.Core.BLL.Applicants;
using CyberSaloon.Core.Repo.Applicants;
using CyberSaloon.Core.DTO.Applicants;
using CyberSaloon.Core.DTO.Applicants.Validation;
using CyberSaloon.Core.BLL.Applications;
using CyberSaloon.Core.Repo.Applications;
using CyberSaloon.Core.DTO.Applications;
using CyberSaloon.Core.DTO.Applications.Validation;
using CyberSaloon.Core.DTO.Common;
using CyberSaloon.Core.Repo.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.JsonPatch;

namespace CyberSaloon.Core.API.Common.Configuration
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddAutomapperProfiles(this IServiceCollection services) => 
            services.AddAutoMapper(typeof(Startup));

        public static IServiceCollection AddDbContext(
                this IServiceCollection services, 
                IConfiguration configuration
            )
        {
            var connection = configuration.GetConnectionString("CoreConnection");
            return
                services.AddDbContext<CyberSaloonDBContext>(it => 
                    it
                        .UseSqlServer(
                            connection, 
                            optionsBuilder => optionsBuilder.MigrationsAssembly("CyberSaloon.Core.API")
                        )
                        .UseLazyLoadingProxies()
                    );
        }

        public static IServiceCollection AddCommonDependencies(this IServiceCollection services) => 
            services
                .AddScoped<IValidator<ResourceParameters>, ResourceParametersValidator>();

        public static IServiceCollection AddArtists(
                this IServiceCollection services, 
                IConfiguration configuration
            ) => 
                services
                    .AddScoped<IArtistsRepository, ArtistsRepository>()
                    .AddScoped<IArtistsService, ArtistsService>()
                    .AddScoped<IValidator<ArtistGetDTO>, ArtistGetDTOValidator>()
                    .AddScoped<IValidator<ArtistPostDTO>, ArtistPostDTOValidator>()
                    .AddScoped<IValidator<ArtistPatchDTO>, ArtistPatchDTOValidator>()
                    .AddScoped<IArtistsDTOValidator, ArtistsDTOValidator>();

        public static IServiceCollection AddArts(
                this IServiceCollection services, 
                IConfiguration configuration
            ) => 
                services
                    .AddScoped<IArtsRepository, ArtsRepository>()
                    .AddScoped<IArtsService, ArtsService>()
                    .AddScoped<IValidator<ArtGetDTO>, ArtGetDTOValidator>()
                    .AddScoped<IValidator<ArtPostDTO>, ArtPostDTOValidator>()
                    .AddScoped<IValidator<JsonPatchDocument<Art>>, ArtPatchDTOValidator>()
                    .AddScoped<IArtsDTOValidator, ArtsDTOValidator>();

        public static IServiceCollection AddApplications(
                this IServiceCollection services, 
                IConfiguration configuration
            ) => 
                services
                    .AddScoped<IApplicationsRepository, ApplicationsRepository>()
                    .AddScoped<IApplicationsService, ApplicationsService>()
                    .AddScoped<IValidator<ApplicationGetDTO>, ApplicationGetDTOValidator>()
                    .AddScoped<IValidator<ApplicationPostDTO>, ApplicationPostDTOValidator>()
                    .AddScoped<IValidator<JsonPatchDocument<Application>>, ApplicationPatchDTOValidator>()
                    .AddScoped<IApplicationsDTOValidator, ApplicationsDTOValidator>();
                
        public static IServiceCollection AddApplicants(
                this IServiceCollection services, 
                IConfiguration configuration
            ) => 
                services
                    .AddScoped<IApplicantsRepository, ApplicantsRepository>()
                    .AddScoped<IApplicantsService, ApplicantsService>()
                    .AddScoped<IValidator<ApplicantGetDTO>, ApplicantGetDTOValidator>()
                    .AddScoped<IValidator<ApplicantPostDTO>, ApplicantPostDTOValidator>()
                    .AddScoped<IValidator<ApplicantPatchDTO>, ApplicantPatchDTOValidator>()
                    .AddScoped<IApplicantsDTOValidator, ApplicantsDTOValidator>();
    }
}