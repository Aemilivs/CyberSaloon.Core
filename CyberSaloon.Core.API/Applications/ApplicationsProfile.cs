using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CyberSaloon.Core.DTO.Applications;
using CyberSaloon.Core.Repo.Applicants;
using CyberSaloon.Core.Repo.Applications;
using CyberSaloon.Core.Repo.Artists;

namespace CyberSaloon.Core.API.Applications
{
    public class ApplicationsProfile : Profile
    {
        private readonly IArtistsRepository _artistsRepository;
        private readonly IApplicationsRepository _applicationsRepository;
        private readonly IApplicantsRepository _applicantsRepository;
        public ApplicationsProfile(
                IArtistsRepository artistsRepository,
                IApplicationsRepository applicationsRepository,
                IApplicantsRepository applicantsRepository
            )
        {
            _artistsRepository =
                artistsRepository ??
                throw new System.ArgumentNullException(nameof(artistsRepository));
            
            _applicantsRepository =
                applicantsRepository ??
                throw new System.ArgumentNullException(nameof(applicantsRepository));

            _applicationsRepository =
                applicationsRepository ??
                throw new System.ArgumentNullException(nameof(applicationsRepository));
        
            CreateMap<Application, ApplicationGetDTO>()
                .ForMember(
                        it => it.Artist, 
                        it => it.MapFrom(that => that.Artist.Alias)
                    )
                .ForMember(
                        it => it.Author, 
                        it => it.MapFrom(that => that.Applicant.Alias)
                    )
                .ForMember(
                        it => it.Art, 
                        it => it.MapFrom(that => that.ArtId)
                    )
                .ForMember(
                        it => it.Applicants, 
                        it => it.MapFrom(that => that.Supporters.Select(applicant => applicant.Id))
                    );

            CreateMap<ApplicationPostDTO, Application>()
                .ForMember(
                        it => it.Id,
                        it => it.MapFrom(that => Guid.NewGuid())
                    )
                .ForMember(
                        it => it.Artist, 
                        it => it.MapFrom(that => _artistsRepository.ReadByAlias(that.Artist))
                    )
                .ForMember(
                        it => it.Applicant, 
                        it => it.MapFrom(that => _applicantsRepository.ReadByAlias(that.Author))
                    )
                .ForMember(
                        it => it.Supporters,
                        it => it.MapFrom(that => new List<Applicant>())
                );
        }
    }
}