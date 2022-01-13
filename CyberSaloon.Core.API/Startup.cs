using System;
using AutoMapper;
using CyberSaloon.Core.API.Applicants;
using CyberSaloon.Core.API.Applications;
using CyberSaloon.Core.API.Artists;
using CyberSaloon.Core.API.Arts;
using CyberSaloon.Core.API.Common.Configuration;
using CyberSaloon.Core.Repo.Applicants;
using CyberSaloon.Core.Repo.Applications;
using CyberSaloon.Core.Repo.Artists;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections.Generic;
using CyberSaloon.Core.API.Common;

namespace CyberSaloon.Core.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(it => 
                    {
                        it.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        it.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    }
                )
                .AddIdentityServerAuthentication(it =>
                    {
                        it.RequireHttpsMetadata = Configuration.GetValue<bool>("Oauth:CyberSaloon:RequireHttpsMetadata");
                        it.Authority = Configuration.GetValue<string>("Oauth:CyberSaloon:Authority");
                        it.ApiName = Configuration.GetValue<string>("Oauth:CyberSaloon:ApiName");
                    }
                );
                
            services.AddAuthorization();

            // TODO: Extract to an extension.
            services.AddSingleton<ILoggerFactory>(LoggerFactory.Create(it => it.AddConsole()));
            
            services
                .AddControllers()
                .AddNewtonsoftJson(
                    it => 
                    {
                        it.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        it.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; 
                    }
                );
            services.AddSwaggerGen(it =>
                {
                    it.SwaggerDoc(
                        "v1", 
                        new OpenApiInfo 
                        { 
                            Title = "CyberSaloon.Core.API", 
                            Version = "v1",
                            Contact =
                                new OpenApiContact
                                {
                                    Email = "Vadim.Emelin@Protonmail.com",
                                    Name = "Vadim Emelin",
                                    Url = new Uri("https://www.linkedin.com/in/vadim-emelin/"),
                                }
                        }
                    );
                    it.AddSecurityDefinition(
                            "oauth2",
                            new OpenApiSecurityScheme
                            {
                                Type = SecuritySchemeType.OAuth2,
                                Flows = 
                                    new OpenApiOAuthFlows
                                    {
                                        Implicit =
                                            new OpenApiOAuthFlow
                                            {
                                                AuthorizationUrl = new Uri(Configuration.GetValue<string>("Oauth:CyberSaloon:Authority"), UriKind.Absolute),
                                                Scopes = new Dictionary<string, string>
                                                {
                                                    { "CyberSaloon.ServerAPI", "" },
                                                    { "openid", "" },
                                                    { "profile", "" }
                                                }
                                            }
                                    }
                            }
                        );
                    it.AddSecurityRequirement(
                            new OpenApiSecurityRequirement
                            {
                                {
                                    new OpenApiSecurityScheme
                                    {
                                        Reference =
                                            new OpenApiReference
                                            {
                                                Type = ReferenceType.SecurityScheme,
                                                Id = "oauth2"
                                            }
                                    },
                                    new[] { "CyberSaloon.ServerAPI", "openid", "profile" }
                                }
                            }
                        );
                }
            );

            services
                .AddDbContext(Configuration)
                .AddAutomapperProfiles()
                .AddCommonDependencies()
                .AddArtists(Configuration)
                .AddArts(Configuration)
                .AddApplicants(Configuration)
                .AddApplications(Configuration);

            // TODO: Shift profile configuration to an extension.
            services
                .AddScoped(
                    provider => new MapperConfiguration(
                        configuration =>
                        {
                            configuration.AddProfile(
                                    new ArtsProfile(
                                            provider.GetService<IArtistsRepository>(),
                                            provider.GetService<IApplicationsRepository>()
                                        )
                                    );
                            configuration.AddProfile(
                                    new ApplicationsProfile(
                                            provider.GetService<IArtistsRepository>(),
                                            provider.GetService<IApplicationsRepository>(),
                                            provider.GetService<IApplicantsRepository>()
                                        )
                                    );
                            configuration.AddProfile(new ApplicantsProfile());
                            configuration.AddProfile(new ArtistsProfile());
                        }                 
                    )
                .CreateMapper());
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(
                it =>
                {
                    it.SwaggerEndpoint("/swagger/v1/swagger.json", "CyberSaloon.Core.API v1");
                    it.OAuthClientId("CyberSaloon.Client");
                    it.OAuthUsePkce();
                    it.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                }
            );
            app.UseRouting();

            app.UseCors(
                it => 
                    it
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(origin => true) 
                        .AllowCredentials()
            );

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(it => it.MapControllers());
        }
    }
}
